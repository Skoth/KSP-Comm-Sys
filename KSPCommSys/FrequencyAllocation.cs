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
