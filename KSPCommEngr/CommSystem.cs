using System.Collections.Generic;
using System.Linq;
using KSP;
using UnityEngine;
using System.Xml.Linq;

namespace KSPCommEngr
{
    public class CommSystem
    {
        public List<CommBlock> blockDiagram = new List<CommBlock>();
        private Vector2 scrollViewVector = Vector2.zero;
        private float left = 210f;
        private float top = 650f;
        private float width = ((float)Screen.width) - 310f;
        private float height = ((float)Screen.height) - 810f;
        private string text = "Inside the scrollview element on Unity within Kerbal Space Program";

        public CommSystem(XDocument xmlCSDoc = null)
        {
            if (xmlCSDoc == null)
            {
                // Simple DSB Receiver
                CommEngrUtils.Log("CommSystem: Allocation for DSB Receiver Start");
                blockDiagram.AddRange(new CommBlock[] {
                    new CommBlock(nodeFace.DataSource, new Rect(400f, 400f, 60f, 60f)),
                    new CommBlock(nodeFace.Multiplier, new Rect(500f, 500f, 60f, 60f)),
                    new CommBlock(nodeFace.LocalOscillator, new Rect(600f, 600f, 60f, 60f)),
                    new CommBlock(nodeFace.LowpassFilter, new Rect(700f, 700f, 60f, 60f)),
                    new CommBlock(nodeFace.DataStore, new Rect(800f, 800f, 60f, 60f)) { }
                });
                CommEngrUtils.Log("CommSystem: Allocation for DSB Receiver End");
            }
        }

        public void DrawCommSystem()
        {
            GUI.Box(new Rect(left, top, width, height), "System Diagram");

            foreach (var block in blockDiagram)
            {
                block.DrawBlockNode();
            }

            scrollViewVector = GUI.BeginScrollView(new Rect(left + 60f, top + 60f, width - 100f, height - 50f), scrollViewVector, new Rect(left, top, 100f, 100f));

            // Inner scrollView Content
            text = GUI.TextArea(new Rect(left, top, 100f, 100f), text);

            GUI.EndScrollView();

            // TODO: add horizontal scrollview for node selection along top of commsystem window
            // TODO: drag and drop placement of block nodes from this horizontal scrollview
            // TODO: dotted grid matrix as background of comm system for "direct" placement of nodes
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

        public CommBlock FindDataSource()
        {
            var qDataSource = from block in blockDiagram
                              where block.Face == nodeFace.DataSource
                              select block;
            if (qDataSource.Count() == 1)
            {
                return qDataSource.First();
            }
            else
            {
                CommEngrUtils.Log("Error at CommSystem.FindDataSource(): one data source must be implemented!");
                return null;
            }

        }
    }
}
