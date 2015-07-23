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
using KSPCommEngr.Extensions;
using KSP.IO;
using System.Linq;

namespace KSPCommEngr
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class FrequencyAllocation : MonoBehaviour
    {
        private static Rect windowPosition = new Rect();
        private GUIStyle windowStyle, labelStyle;
        private bool hasInitStyles = false;
        public Texture2D image = new Texture2D(400, 100); //400 pixels wide by 100 pixels tall

        public void Awake()
        {
            CommEngrLog.Log("Frequency Allocation MonoBehaviour Activated");
        }

        public void Start()
        {
            // Signal tests with sinusoid
            Signal q = new Signal(x => Mathf.Sin(x));
            CommEngrLog.Log("Frequency Allocation - Sine Signal generated.");
            //q.DisplaySignal();

            // Test channel noise distortion
            q = Channel.AdditiveNoise(q);
            CommEngrLog.Log("Frequency Allocation - Channel Noise added to Signal.");
            //q.DisplaySignal();

            if (!hasInitStyles) InitStyles();

            drawImage();
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }

        public void Update()
        {
            //CommSysLog.Log("Mouse position (" + Mouse.screenPos.x.ToString() + ", " + Mouse.screenPos.y.ToString() + ")");
        }

        private void OnDraw()
        {

            windowPosition = GUILayout.Window(10, windowPosition, OnWindow, "Frequency Allocation Chart", windowStyle);

            if (windowPosition.x == 0f && windowPosition.y == 0f)
                windowPosition = windowPosition.CenterScreen();
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginVertical();
                GUI.DrawTexture(new Rect(10f, 10f, 400f, 100f), image, ScaleMode.StretchToFill);
                GUILayout.Label("Frequency in Hz", labelStyle);
            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        private void InitStyles()
        {
            windowStyle = new GUIStyle(HighLogic.Skin.window);
            windowStyle.fixedWidth = 900f;
            windowStyle.fixedHeight = 900f;

            labelStyle = new GUIStyle(HighLogic.Skin.label);
            labelStyle.stretchWidth = true;
            //labelStyle.DrawCursor

            hasInitStyles = true;
        }

        // Code provided from KSP Wiki @: http://wiki.kerbalspaceprogram.com/wiki/Module_code_examples
        // Special thanks to the author of the "Drawing and displaying a 2D image" snippet
        void drawImage()
        {
            // Set all the pixels to black
            int size = image.height * image.width;

            Color[] colors = Enumerable.Repeat(Color.black, size).ToArray();
            image.SetPixels(colors);

            int lineWidth = 3; //draw a curve 3 pixels wide
            for (int x = 0; x < image.width; x++)
            {
                int fx = f(x);
                for (int y = fx; y < fx + lineWidth; y++)
                {
                    image.SetPixel(x, y, Color.red);
                    CommEngrLog.Log("drawImage() - (" + x.ToString() + ", " + y.ToString() + ")");
                }
            }

            image.Apply();
        }

        //the function to plot:
        int f(int x)
        {
            return 2 * x;
        }
    }
}
