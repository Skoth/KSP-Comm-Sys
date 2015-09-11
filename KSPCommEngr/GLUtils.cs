﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;

namespace KSPCommEngr
{
    // MechJeb GLUtils class to test rendering
    public class GLUtils
    {
        static Material _material;
        static Material material
        {
            get
            {
                if (_material == null) _material = new Material(Shader.Find("Particles/Additive"));
                return _material;
            }
        }

        public static void GLTriangleMap(Vector3d[] worldVertices, Color c)
        {
            GL.PushMatrix();
            material.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.TRIANGLES);
            GL.Color(c);
            GLVertexMap(worldVertices[0]);
            GLVertexMap(worldVertices[1]);
            GLVertexMap(worldVertices[2]);
            GL.End();
            GL.PopMatrix();
        }

        public static void GLVertexMap(Vector3d worldPosition)
        {
            Vector3 screenPoint = PlanetariumCamera.Camera.WorldToScreenPoint(ScaledSpace.LocalToScaledSpace(worldPosition));
            GL.Vertex3(screenPoint.x / Camera.main.pixelWidth, screenPoint.y / Camera.main.pixelHeight, 0);
        }

        //Tests if byBody occludes worldPosition, from the perspective of the planetarium camera
        public static bool IsOccluded(Vector3d worldPosition, CelestialBody byBody)
        {
            if (Vector3d.Distance(worldPosition, byBody.position) < byBody.Radius - 100) return true;

            Vector3d camPos = ScaledSpace.ScaledToLocalSpace(PlanetariumCamera.Camera.transform.position);

            if (Vector3d.Angle(camPos - worldPosition, byBody.position - worldPosition) > 90) return false;

            double bodyDistance = Vector3d.Distance(camPos, byBody.position);
            double separationAngle = Vector3d.Angle(worldPosition - camPos, byBody.position - camPos);
            double altitude = bodyDistance * Math.Sin(Math.PI / 180 * separationAngle);
            return (altitude < byBody.Radius);
        }

        public static void DrawConnection(Vector2 ptA, Vector2 ptB, Color color, float width)
        {
            float iA = Vector2.Angle(ptA, ptB);
            Color savedColor = GUI.color;
            Matrix4x4 savedMatrix = GUI.matrix;
            Vector2 rectPtA = new Vector2(ptA.x, Screen.height - ptA.y);
            float angle = (ptB.y < rectPtA.y) ? -Vector2.Angle(ptB - rectPtA, Vector2.right) : Vector2.Angle(ptB - rectPtA, Vector2.right);
            GUIUtility.RotateAroundPivot(angle, rectPtA);
            float mag = (ptB - rectPtA).magnitude;
            Texture2D lineTex = new Texture2D(128, 128);
            for(int i = 0; i < lineTex.width; ++i)
            {
                //
                for(int j = 0; j < lineTex.height/2; ++j)
                {
                    lineTex.SetPixel(i, j, new Color(j, j, j, 1f));
                }
                for (int j = lineTex.height / 2; j < lineTex.height; ++j)
                {
                    lineTex.SetPixel(i, j, new Color(lineTex.height - j, lineTex.height - j, lineTex.height - j, 1f));
                }
            }
            lineTex.Apply();
            GUI.DrawTexture(new Rect(ptA.x, rectPtA.y, mag, width), lineTex);
            GUI.color = savedColor;
            GUI.matrix = savedMatrix;
        }
    }
}
