#region license

/* The MIT License (MIT)

 * Copyright (c) 2015 Skoth

 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using KSP;
using KSPCommEngr.Extensions;

namespace KSPCommEngr
{
    public enum nodeFace
    {
        BandpassFilter,
        DataSource,
        DataStore,
        HighpassFilter,
        LocalOscillator,
        LowpassFilter,
        Multiplier,
        Receiver,
        Transmitter
    }

    public class EdgeNode
    {
        public static Dictionary<string, Rect> VertexDistance = new Dictionary<string, Rect>()
        {
            { "Top", new Rect(30f, 0f, 0f, 0f) },
            { "Right", new Rect(60f, 30f, 0f, 0f) },
            { "Bottom", new Rect(30f, 60f, 0f, 0f) },
            { "Left", new Rect(0f, 30f, 0f, 0f) }
        };
        public CommBlockNode Connection = null;

        public Rect Position { get; set; }
        public EdgeNode(Rect pos, CommBlockNode conn = null)
        {
            Position = pos;
            if (conn != null) Connection = conn;
        }
    }

    // Non-GUI ops specified in IVertex interface
    public class CommBlockNode
    {
        public nodeFace Face { get; set; }
        public Texture2D Icon;
        public Rect Position;
        public Dictionary<string, EdgeNode> EdgeNodes = new Dictionary<string, EdgeNode>()
        {
            { "Top", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Right", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Bottom", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Left", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
        };
        public IEnumerable<CommBlockNode> AdjacentBlocks;
        private Vector2? offsetPos = null;
        private bool blockSelected = false;
        private bool edgeNodeHovered = false;
        private bool edgeNodeSelected = false;
        private int guiDepth = 2;

        public CommBlockNode(nodeFace nf, Rect pos)
        {
            Face = nf;
            Position = pos;
            Icon = GameDatabase.Instance.GetTexture(String.Format("CommEngr/Textures/{0}", Face.ToString()), false);
            
            UpdateEdgeNodes();
        }

        private static GUIStyle GetBlockStyle(Texture2D tex)
        {
            return new GUIStyle()
            {
                active = HighLogic.Skin.button.active,
                alignment = TextAnchor.MiddleCenter,
                border = new RectOffset(0, 0, 0, 0),
                clipping = TextClipping.Clip,
                contentOffset = new Vector2(0, 0),
                fixedHeight = tex.height,
                fixedWidth = tex.width,
                focused = HighLogic.Skin.window.focused,
                font = null,
                hover = HighLogic.Skin.button.hover,
                imagePosition = ImagePosition.ImageOnly,
                margin = new RectOffset(0, 0, 0, 0),
                name = tex.name,
                normal = HighLogic.Skin.button.normal,
                onActive = new GUIStyleState() { background = HighLogic.Skin.button.active.background, textColor = Color.white },
                onFocused = new GUIStyleState() { background = HighLogic.Skin.button.focused.background, textColor = Color.white },
                onHover = new GUIStyleState() { background = HighLogic.Skin.button.hover.background, textColor = Color.white },
                onNormal = new GUIStyleState() { background = HighLogic.Skin.button.normal.background, textColor = Color.white },
                overflow = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                richText = false,
                stretchHeight = false,
                stretchWidth = false,
                wordWrap = true
            };
        }

        
        public void DrawBlockNode()
        {
            // Draggable Block Node
            if (Position.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && !(edgeNodeSelected || edgeNodeHovered))
            {
                blockSelected = true;
            }

            if (blockSelected && Event.current.type == EventType.MouseUp)
            {
                blockSelected = false;
                offsetPos = null;
            }
            
            if (blockSelected)
            {
                guiDepth = 1;

                // Enables drag and drop from any section of the block clicked
                if (offsetPos == null)
                    offsetPos = new Vector2(Mouse.screenPos.x - Position.x, Mouse.screenPos.y - Position.y);

                Position.x = Mouse.screenPos.x - offsetPos.Value.x;
                Position.y = Mouse.screenPos.y - offsetPos.Value.y;
                UpdateEdgeNodes();
            }
            else guiDepth = 2;

            // Render Block Node, followed by its connectors
            GUI.depth = guiDepth;
            if (GUI.Button(Position, Icon, GetBlockStyle(Icon)))
            {
                CommEngrUtils.Log(String.Format("Block Node with Rect({0}, {1}, {2}, {3}) clicked!",
                    Position.x, Position.y, Position.width, Position.height));
            }

            // Selectable Edge Nodes
            GUI.depth = 0;
            foreach (var edgeNode in EdgeNodes)
            {
                if (edgeNode.Value.Position.Contains(Event.current.mousePosition))
                    edgeNodeHovered = true;
                else
                    edgeNodeHovered = false;

                if (edgeNodeHovered && Event.current.type == EventType.MouseDown)
                    edgeNodeSelected = true;

                if (edgeNodeSelected && Event.current.type == EventType.MouseUp)
                {
                    screenPt1 = new Vector2(edgeNode.Value.Position.x, edgeNode.Value.Position.y);
                    screenPt2 = new Vector2(Mouse.screenPos.x, Mouse.screenPos.y);

                    callDrawEdge = true;
                    CommEngrUtils.Log("DrawLine initiating with red line...");
                    

                    //DrawConnection(new Vector2(edgeNode.Value.Position.x, edgeNode.Value.Position.y), Mouse.screenPos, Color.red, 3.0f);
                    //DrawLine(new Vector2(edgeNode.Value.Position.x, edgeNode.Value.Position.y), Mouse.screenPos, Color.red, 3f);
                    //CommEngrUtils.Log("GL Lines initiating with blue line...");
                    
                    //GL.PushMatrix();
                    //GL.Begin(GL.LINES);
                    //    GL.Color(Color.blue);
                    //    GL.Vertex(new Vector3(edgeNode.Value.Position.x, edgeNode.Value.Position.y, 0f));
                    //    GL.Vertex(new Vector3(Mouse.screenPos.x, Mouse.screenPos.y, 0f));
                    //GL.End();
                    //GL.PopMatrix();
                    //CommEngrUtils.Log("GUI Label initiating with green line...");
                    //Texture2D tempTex = new Texture2D(5, 5);
                    //tempTex.SetPixels(Enumerable.Repeat(Color.green, tempTex.width * tempTex.height).ToArray());
                    //GUIUtility.ScaleAroundPivot(new Vector2(), new Vector2());
                    //GUI.Label(new Rect(200, 200, 10, 10), tempTex);
                    edgeNodeHovered = false;
                    edgeNodeSelected = false;
                }

                // Render Edge Node Buttons
                // Test opacity rendering
                Color temp = GUI.color;
                GUI.color = new Color(temp.r, temp.g, temp.b, 0.5f);
                if (GUI.Button(edgeNode.Value.Position, "EN"))
                {
                    CommEngrUtils.Log(String.Format("Edge Node with Rect({0}, {1}, {2}, {3}) clicked!",
                        edgeNode.Value.Position.x, edgeNode.Value.Position.y,
                        edgeNode.Value.Position.width, edgeNode.Value.Position.height));
                }
                GUI.color = temp;

                if (callDrawEdge) CommBlockNode.DrawConnection(screenPt1, screenPt2, Color.red, 5f);
            }
        }
        private static Vector2 screenPt1;
        private static Vector2 screenPt2;
        private static void DrawEdge()
        {
            //if (screenPt1 != null && screenPt2 != null) {
            //    GL.PushMatrix();
            //    GL.Begin(GL.LINES);
            //        GL.Color(Color.red);
            //        GL.Vertex3(screenPt1.x, screenPt1.y, screenPt1.z);
            //        GL.Vertex3(screenPt2.x, screenPt2.y, screenPt2.z);
            //    GL.End();
            //    GL.PopMatrix();
            //}
        }

        private void UpdateEdgeNodes()
        {
            foreach (var edgeNode in EdgeNodes)
            {
                EdgeNodes[edgeNode.Key].Position = new Rect(
                    Position.x + EdgeNode.VertexDistance[edgeNode.Key].x - edgeNode.Value.Position.width / 2,
                    Position.y + EdgeNode.VertexDistance[edgeNode.Key].y - edgeNode.Value.Position.height / 2,
                    EdgeNodes[edgeNode.Key].Position.width,
                    EdgeNodes[edgeNode.Key].Position.height
                );
            }
        }


        private static Rect lineRect = new Rect(0, 0, 1, 1);
        private static Texture2D lineTex = null;
        private static Material blitMat = null;
        private static Material blendMat = null;
        private bool callDrawEdge = false;

        private static void initConnection()
        {
            lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            lineTex.SetPixel(0, 1, Color.red);
            lineTex.Apply();
            blitMat = (Material)typeof(GUI).GetMethod("get_blitMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            blendMat = (Material)typeof(GUI).GetMethod("get_blendMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            return;
        }
        private static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            if (lineTex == null) initConnection();
            float dx = pointB.x - pointA.x;
            float dy = pointB.y - pointA.y;
            float len = Mathf.Sqrt(dx * dx + dy * dy);
            if (len < 0.001f) return;
            Texture2D tex = lineTex;
            Material mat = blitMat;
            float wdx = width * dy / len;
            float wdy = width * dx / len;
            /*
                [ 00 , 01, 02, 03 ]
                [ 10 , 11, 12, 13 ]
                [ 20 , 21, 22, 23 ]
                [ 30 , 31, 32, 33 ]
            */
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.m00 = dx;
            matrix.m01 = -wdx;
            matrix.m03 = pointA.x + 0.5f * wdx;
            matrix.m10 = dy;
            matrix.m11 = wdy;
            matrix.m13 = pointA.y - 0.5f * wdy;
            GL.PushMatrix();
            GL.MultMatrix(matrix);
            GUI.color = color;
            GUI.DrawTexture(lineRect, tex);
            GL.PopMatrix();
        }

        public static void DrawConnection(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            // Save the current GUI matrix, since we're going to make changes to it.
            Matrix4x4 matrix = GUI.matrix;

            // Generate a single pixel texture if it doesn't exist
            if (!lineTex) { lineTex = new Texture2D(1, 1); }

            // Store current GUI color, so we can switch it back later,
            // and set the GUI color to the color parameter
            Color savedColor = GUI.color;
            GUI.color = color;

            // Determine the angle of the line.
            float angle = Vector3.Angle(pointB - pointA, Vector2.right);

            // Vector3.Angle always returns a positive number.
            // If pointB is above pointA, then angle needs to be negative.
            if (pointA.y > pointB.y) { angle = -angle; }

            // Use ScaleAroundPivot to adjust the size of the line.
            // We could do this when we draw the texture, but by scaling it here we can use
            //  non-integer values for the width and length (such as sub 1 pixel widths).
            // Note that the pivot point is at +.5 from pointA.y, this is so that the width of the line
            //  is centered on the origin at pointA.
            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));

            // Set the rotation for the line.
            //  The angle was calculated with pointA as the origin.
            GUIUtility.RotateAroundPivot(angle, pointA);

            // Finally, draw the actual line.
            // We're really only drawing a 1x1 texture from pointA.
            // The matrix operations done with ScaleAroundPivot and RotateAroundPivot will make this
            //  render with the proper width, length, and angle.
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);

            // We're done.  Restore the GUI matrix and GUI color to whatever they were before.
            GUI.matrix = matrix;
            GUI.color = savedColor;
        }
    }
}
