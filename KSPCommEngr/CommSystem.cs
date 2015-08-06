using System.Collections.Generic;
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
            blockDiagram.Add(new CommBlockNode(nodeFace.DataSource, new Rect(400f, 400f, 30f, 30f)));
            blockDiagram.Add(new CommBlockNode(nodeFace.LocalOscillator, new Rect(500f, 500f, 60f, 60f)));
            blockDiagram.Add(new CommBlockNode(nodeFace.BandpassFilter, new Rect(100f, 100f, 100f, 100f)));
        }
        
        public void OnGUI()
        {
            foreach(var block in blockDiagram)
            {
                block.GUIHandler();
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
                if(node.Equals(new Object()))
                    return node;
            }
            return null;
        }
    }
}
