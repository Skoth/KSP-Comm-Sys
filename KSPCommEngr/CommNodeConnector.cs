using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;

namespace KSPCommEngr
{
    public class CommNodeConnector
    {
        // Refactor into constants in CommEngrUtils?
        public static Dictionary<string, Rect> VertexDistance = new Dictionary<string, Rect>()
        {
            { "Top", new Rect(30f, 0f, 0f, 0f) },
            { "Right", new Rect(60f, 30f, 0f, 0f) },
            { "Bottom", new Rect(30f, 60f, 0f, 0f) },
            { "Left", new Rect(0f, 30f, 0f, 0f) }
        };

        public CommBlockNode LConnectedNode = null;
        public CommBlockNode RConnectedNode = null;

        public Rect Position { get; set; }
        public CommNodeConnector(Rect pos, CommBlockNode lNode = null, CommBlockNode rNode = null)
        {
            Position = pos;
            LConnectedNode = lNode;
            RConnectedNode = rNode;
            if (LConnectedNode != null && RConnectedNode != null)
                Debug.Log("DrawConnection()");
                //DrawConnection();
        }
    }
}
