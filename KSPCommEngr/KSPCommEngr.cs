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
using KSPCommSys.Extensions;
using KSP.IO; // namespace for where the plugin configuration class is kept

// Code inspired by Cybutek's YouTube tutorials: https://www.youtube.com/watch?v=ilWZjYx7brE

namespace KSPCommSys
{
    public class KSPCommEngr : PartModule
    {
        private static Rect windowPosition = new Rect();
        private GUIStyle windowStyle, labelStyle;
        private bool hasInitStyles = false;

        public override void OnStart(PartModule.StartState state)
        {
            if (state != StartState.Editor) // if not in editor, then in flight 
            {
                if (!hasInitStyles) InitStyles();
                RenderingManager.AddToPostDrawQueue(0, OnDraw);
            }
        }

        public void FixedUpdate()
        {
            CommEngrLog.Log("Test Part FixedUpdate()");
            //Debug.Log("@{ FixedUpdate: Custom Part Live }@");
        }

        public override void OnSave(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<KSPCommEngr>();

            config.SetValue("Window Position", windowPosition);
            config.save();
        }

        public override void OnLoad(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<KSPCommEngr>();

            config.load();
            windowPosition = config.GetValue<Rect>("Window Position");
        }

        public override void OnUpdate()
        {

            if (this.vessel == FlightGlobals.ActiveVessel)
            {
                this.vessel.rigidbody.AddRelativeForce(Vector3.up * 500f * FlightInputHandler.state.mainThrottle);
                CommEngrLog.Log(FlightInputHandler.state.mainThrottle.ToString());
            }
        }

        private void OnDraw()
        {
            // if the current vessel is the same as the one the part is on
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            { 
                windowPosition = GUILayout.Window(10, windowPosition, OnWindow, "Noise Profile", windowStyle);

                if (windowPosition.x == 0f && windowPosition.y == 0f)
                    windowPosition = windowPosition.CenterScreen();
            }
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("", labelStyle);
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        private void InitStyles()
        {
            windowStyle = new GUIStyle(HighLogic.Skin.window);
            windowStyle.fixedWidth = 250f;

            labelStyle = new GUIStyle(HighLogic.Skin.label);
            labelStyle.stretchWidth = true;
            
            hasInitStyles = true;
        }
    }
}
