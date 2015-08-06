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

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KSP;
using KSPCommEngr.Extensions;

namespace KSPCommEngr
{
    public enum nodeFace
    {
        BandpassFilter,
        DataSource, // spec for satellite vs ground?
        HighpassFilter,
        LocalOscillator,
        LowpassFilter,
        Multiplier,
        Receiver,
        Transmitter
    }
    
    public class EdgeNode
    {
        public static Dictionary<string, Vector2> VertexDistance = new Dictionary<string, Vector2>()
        {
            { "Top", new Vector2(5f, 0f) },
            { "Right", new Vector2(10f, 5f) },
            { "Bottom", new Vector2(5f, 10f) },
            { "Left", new Vector2(0f, 5f) }
        };

        public Rect Position { get; set; }
        public EdgeNode(Rect pos)
        {
            Position = pos;
        }
    }

    // Non-GUI ops specified in IVertex interface
    public class CommBlockNode
    {
        public nodeFace Face { get; set; }
        public Rect Position = new Rect(30f, 30f, 10f, 10f);
        public Dictionary<string, EdgeNode> EdgeNodes = new Dictionary<string, EdgeNode>()
        {
            { "Top", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Right", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Bottom", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Left", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
        };
        private bool selected = false;

        public CommBlockNode(nodeFace nf, Rect pos)
        {
            Face = nf;
            Position = pos;
            UpdateEdgeNodes();
        }

        public void GUIHandler()
        {
            // Draggable Block Node
            if (Position.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                selected = true;
            }

            if (selected && Event.current.type == EventType.MouseUp)
            {
                selected = false;
            }

            if (selected)
            {
                Position.x = Mouse.screenPos.x - Position.width / 2;
                Position.y = Mouse.screenPos.y - Position.height / 2;
                UpdateEdgeNodes();
            }

            // Render Block Node, followed by its connectors
            if (GUI.Button(Position, Face.ToString()))
            {
                CommEngrUtils.Log("Node clicked!");
            }

            foreach(var edgeNode in EdgeNodes)
            {
                if(GUI.Button(edgeNode.Value.Position, "EN"))
                {
                    CommEngrUtils.Log("EN Clicked!");
                }
            }
        }

        private void UpdateEdgeNodes()
        {
            foreach(var edgeNode in EdgeNodes)
            {
                EdgeNodes[edgeNode.Key].Position = new Rect(
                    Position.x + EdgeNode.VertexDistance[edgeNode.Key].x,
                    Position.y + EdgeNode.VertexDistance[edgeNode.Key].y,
                    10f, 10f);
            }
        }


    }
}
