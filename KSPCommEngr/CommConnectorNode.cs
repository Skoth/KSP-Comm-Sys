using UnityEngine;
using KSP;

namespace KSPCommEngr
{
    public class CommConnectorNode
    {
        public enum States
        {
            Default, // (half opacity): cursor not in range and not ClickAndDrag()
            CursorNearby, // (opaque): Eligible for ClickAndDrag()
            ClickAndDrag
        }
        public enum BlockNodeSector {
            Top,
            Right,
            Bottom,
            Left
        }
        public Rect currentPosition { get; set; }
        public Rect nodePosition
        {
            get { return nodePosition; }
            set
            {   // Only update node position if it comes as a result of the
                // CommBlockNode being dragged and dropped
                bool windowFinishedDragging = true;
                if (windowFinishedDragging) nodePosition = value;
            }
        }
        public GUIElement placeholder { get; set; } // faded node present during drag
        private GUIStyle opacity;
        private GUIStyle color;
        private BlockNodeSector sector;
        public CommConnectorNode()
        {
            sector = BlockNodeSector.Bottom;
        }

        public void OnClickAndDragStart()
        {
            placeholder.active = false;
            SetVisibility(States.ClickAndDrag);
        }

        public void OnDrag()
        {

        }

        public void OnClickAndDragEnd()
        {
            SetVisibility(States.Default);
        }

        private void SetVisibility(States state) {
            switch (state)
            {
                case States.ClickAndDrag:
                    placeholder.guiText.text = "opacity:0.5";
                    opacity = new GUIStyle(); // 1.0
                    color = new GUIStyle(); // color green
                    break;
                case States.CursorNearby:
                    break;
                case States.Default:
                default:
                    break;
            }
        }
    }
}
