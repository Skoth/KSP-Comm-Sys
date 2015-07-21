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

using UnityEngine;
using System.Collections;
using KSPCommSys.Extensions;
using KSP.IO;

namespace KSPCommSys
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, true)]
    public class FrequencyAllocation : MonoBehaviour
    {
        private static Rect windowPosition = new Rect();
        private GUIStyle windowStyle, labelStyle;
        private bool hasInitStyles = false;

        public void Awake() {
            CommSysLog.Log("Frequency Allocation MonoBehaviour Activated");
        }

        public void Start()
        {
            // Signal tests with sinusoid
            Signal q = new Signal(x => Mathf.Sin(x));
            CommSysLog.Log("Frequency Allocation - Sine Signal generated; displayed next:");
            q.DisplaySignal();

            // Test channel noise distortion
            q = Channel.AdditiveNoise(q);
            CommSysLog.Log("Frequency Allocation - Channel Noise added to Signal ; displayed next:");
            q.DisplaySignal();

            if (!hasInitStyles) InitStyles();
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }

        public void Update()
        {
            //CommSysLog.Log("Mouse position (" + Mouse.screenPos.x.ToString() + ", " + Mouse.screenPos.y.ToString() + ")");
        }

        private void OnDraw()
        {
            windowPosition = GUILayout.Window(10, windowPosition, OnWindow, "This is a title", windowStyle);

            if (windowPosition.x == 0f && windowPosition.y == 0f)
                windowPosition = windowPosition.CenterScreen();
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Frequency Allocation Chart", labelStyle);
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        private void InitStyles()
        {
            windowStyle = new GUIStyle(HighLogic.Skin.window);
            windowStyle.fixedWidth = 500f;

            labelStyle = new GUIStyle(HighLogic.Skin.label);
            labelStyle.stretchWidth = true;

            hasInitStyles = true;
        }
    }
}
