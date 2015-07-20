using UnityEngine;
using KSPCommSys.Extensions;
using KSP.IO; // namespace for where the plugin configuration class is kept

// Code inspired by Cybutek's YouTube tutorials: https://www.youtube.com/watch?v=ilWZjYx7brE

namespace KSPCommSys
{
    public class KSPCommSys : PartModule
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
            Debug.Log("@{ FixedUpdate: Custom Part Live }@");
        }

        public override void OnSave(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<KSPCommSys>();

            config.SetValue("Window Position", windowPosition);
            config.save();
        }

        public override void OnLoad(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<KSPCommSys>();

            config.load();
            windowPosition = config.GetValue<Rect>("Window Position");
        }

        public override void OnUpdate()
        {

            if (this.vessel == FlightGlobals.ActiveVessel)
            {
                this.vessel.rigidbody.AddRelativeForce(Vector3.up * 500f * FlightInputHandler.state.mainThrottle);
                Debug.Log("@{ " + FlightInputHandler.state.mainThrottle.ToString() + " }@");
            }
        }

        private void OnDraw()
        {
            // if the current vessel is the same as the one the part is on
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            { 
                windowPosition = GUILayout.Window(10, windowPosition, OnWindow, "This is a title", windowStyle);

                if (windowPosition.x == 0f && windowPosition.y == 0f)
                    windowPosition = windowPosition.CenterScreen();
            }
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("This is a label", labelStyle);
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
