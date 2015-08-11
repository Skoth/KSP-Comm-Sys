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

        public void Awake()
        {
            CommEngrUtils.Log("Ground Station Activated");
        }

        public void Start()
        {
            // Draw system overview window
            RenderingManager.AddToPostDrawQueue(0, OnDraw);


            // TODO add hide/show button 
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
