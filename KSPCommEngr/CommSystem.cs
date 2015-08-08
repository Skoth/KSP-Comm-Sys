using System.Collections.Generic;
using System.Linq;
using KSP;
using UnityEngine;

namespace KSPCommEngr
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class CommSystem : MonoBehaviour
    {
        public List<CommBlockNode> blockDiagram = new List<CommBlockNode>();

        public void Awake()
        {
            CommEngrUtils.Log("CommSystem Awake()");
        }

        public void Start()
        {
            CommEngrUtils.Log("CommSystem Start()");
            blockDiagram.Add(new CommBlockNode(nodeFace.DataSource, new Rect(400f, 400f, 60f, 60f)));
            blockDiagram.Add(new CommBlockNode(nodeFace.LocalOscillator, new Rect(430f, 430f, 60f, 60f)));
            blockDiagram.Add(new CommBlockNode(nodeFace.BandpassFilter, new Rect(460f, 460f, 60f, 60f)));
        }
        
        public void OnGUI()
        {
            GUI.Box(new Rect(210f, 850f, ((float)Screen.width) - 310f, ((float)Screen.height) - 810f), "System Diagram");

            foreach(var block in blockDiagram)
            {
                block.DrawBlockNode();
                foreach(var adjacentBlock in block.AdjacentBlocks)
                {
                    //// block.Position to adjacentBlock
                    //Rect temp = new Rect(
                    //    block.Position.left, block.Position.top,
                    //    Mathf.Abs(block.Position.left - 
                    //);
                    //GUI.DrawTexture(temp, )
                }
            }
            //foreach(var blockEdge in blockDiagram.Select(n => n.Position))
            //{
            //    GUI.DrawTexture(blockEdge, new Texture(), ScaleMode.ScaleToFit, true, 1.0f);
            //}
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
                if(node.Equals(new Object()))
                    return node;
            }
            return null;
        }
    }
}
