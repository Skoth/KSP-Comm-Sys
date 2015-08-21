using System.Collections.Generic;
using System.Linq;
using KSP;
using UnityEngine;
using System.Xml.Linq;

namespace KSPCommEngr
{
    public class CommSystem
    {
        public List<CommBlockNode> blockDiagram = new List<CommBlockNode>();

        public CommSystem(XDocument xmlCSDoc = null)
        {
            if (xmlCSDoc == null)
            {
                // Simple DSB Receiver
                CommEngrUtils.Log("CommSystem: Allocation for DSB Receiver Start");
                blockDiagram.AddRange(new CommBlockNode[] {
                    new CommBlockNode(nodeFace.DataSource, new Rect(400f, 400f, 60f, 60f)),
                    new CommBlockNode(nodeFace.Multiplier, new Rect(500f, 500f, 60f, 60f)),
                    new CommBlockNode(nodeFace.LocalOscillator, new Rect(600f, 600f, 60f, 60f)),
                    new CommBlockNode(nodeFace.LowpassFilter, new Rect(700f, 700f, 60f, 60f)),
                    new CommBlockNode(nodeFace.DataStore, new Rect(800f, 800f, 60f, 60f)) { }
                });
                CommEngrUtils.Log("CommSystem: Allocation for DSB Receiver End");
            }
        }

        public void DrawCommSystem()
        {
            GUI.Box(new Rect(210f, 850f, ((float)Screen.width) - 310f, ((float)Screen.height) - 810f), "System Diagram");

            foreach (var block in blockDiagram)
            {
                block.DrawBlockNode();
            }
        }

        public Signal Cursor
        {
            get
            {
                return new Signal();
            }
            set
            {

            }
        }

        public CommBlockNode FindDataSource()
        {
            foreach (var node in blockDiagram)
            {
                if (node.Equals(new Object()))
                    return node;
            }
            return null;
        }
    }
}
