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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;

namespace KSPCommEngr
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class GroundStation : MonoBehaviour
    {
        private Rect commSystemPosition = new Rect(200f, 800f, ((float)Screen.width) - 300f, ((float)Screen.height) - 800f);
        private CommSystem groundStationCS = null;
        private CommSystem satelliteCS = null;
        private bool click1 = false;
        private Vector2 vClick1;
        private Vector2 vClick11;
        private bool click2 = false;
        private Vector2 vClick2;
        private Vector2 vClick22;

        public void Awake()
        {
            CommEngrUtils.Log("Ground Station Activated");
        }

        public void Start()
        {
            // Draw system overview window
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
            groundStationCS = new CommSystem();
        }
        public void Update()
        {
            if (Mouse.Left.GetClick())
            {
                vClick1 = Mouse.screenPos;
                vClick11 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                click1 = true;
                CommEngrUtils.Log(String.Format("Mouse: ({0}, {1})", Mouse.screenPos.x, Mouse.screenPos.y));
            }
            if (Mouse.Right.GetClick())
            {
                vClick2 = Mouse.screenPos;
                vClick22 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                click2 = true;
            }
        }

        public void OnGUI()
        {
            if (groundStationCS != null) groundStationCS.DrawCommSystem();
            if (satelliteCS != null) satelliteCS.DrawCommSystem();
            if (click1 && click2)
            {
                CommEngrUtils.Log("-DrawConnection() for 2 general mouse clicks!");
                Vector2 guiPt1 = GUIUtility.ScreenToGUIPoint(vClick1);
                Vector2 guiPt2 = GUIUtility.ScreenToGUIPoint(vClick2);
                //CommEngrUtils.Log(String.Format("Rpts: <{0}, {1}>, <{2}, {3}>", vClick1.x, vClick1.y, vClick2.x, vClick2.y));
                //CommEngrUtils.Log(String.Format("Gpts: <{0}, {1}>, <{2}, {3}>", guiPt1.x, guiPt1.y, guiPt2.x, guiPt2.y));
                //CommBlockNode.DrawConnection(guiPt1, guiPt2, Color.red, 3f);

                Vector2 guiPt11 = GUIUtility.ScreenToGUIPoint(vClick11);
                Vector2 guiPt22 = GUIUtility.ScreenToGUIPoint(vClick22);
                //CommEngrUtils.Log(String.Format("R2pts: <{0}, {1}>, <{2}, {3}>", vClick11.x, vClick11.y, vClick22.x, vClick22.y));
                //CommEngrUtils.Log(String.Format("G2pts: <{0}, {1}>, <{2}, {3}>", guiPt11.x, guiPt11.y, guiPt22.x, guiPt22.y));
                //CommBlockNode.DrawConnection(guiPt11, guiPt22, Color.red, 3f);

                click1 = false;
                click2 = false;
            }
        }

        private void OnDraw()
        {
            commSystemPosition = GUILayout.Window(commSystemPosition.GetHashCode(), commSystemPosition, CommSystemWindow, "System Overview", HighLogic.Skin.window);
        }

        private void CommSystemWindow(int winId)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Button("Antenna <|");
            GUILayout.Button("Local Oscillator (~)");
            GUILayout.Button("Mixer (X)");
            GUILayout.EndHorizontal();
            GUILayout.Button("Hide");
            GUILayout.EndVertical();
        }
    }
}
