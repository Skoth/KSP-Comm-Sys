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
        public Texture2D grid;

        public void Awake()
        {
            CommEngrUtils.Log("Ground Station Activated");
        }

        public void Start()
        {
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
            groundStationCS = new CommSystem();
        }
        public void Update()
        {

        }

        public void OnGUI()
        {
            if (groundStationCS != null) groundStationCS.DrawCommSystem();
            if (satelliteCS != null) satelliteCS.DrawCommSystem();
        }

        private void OnDraw()
        {
            CommEngrUtils.Log("DrawPath()");
            GLUtils.GLTriangleMap(new Vector3d[]
            {
                new Vector3d(0d, 100d, -10d),
                new Vector3d(50d, 100d, 10d),
                new Vector3d(-50d, -100d, 0d)
            }, Color.magenta);

            commSystemPosition = GUILayout.Window(commSystemPosition.GetHashCode(), commSystemPosition, CommSystemWindow, "System Overview", HighLogic.Skin.window);
        }

        private void CommSystemWindow(int winId)
        {
            Texture2D tex = new Texture2D(20, 20);
            tex.SetPixels(Enumerable.Repeat(Color.white, 20*20).ToArray());
            tex.SetPixels(0, 9, 20, 1, Enumerable.Repeat(Color.black, 5).ToArray());
            tex.SetPixels(9, 0, 1, 20, Enumerable.Repeat(Color.black, 5).ToArray());
            GUILayout.BeginVertical();
            GUILayout.Box(tex);
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
