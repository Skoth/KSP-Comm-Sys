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
using System.IO;
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
        public static Dictionary<string, Rect> VertexDistance = new Dictionary<string, Rect>()
        {
            { "Top", new Rect(30f, 0f, 0f, 0f) },
            { "Right", new Rect(60f, 30f, 0f, 0f) },
            { "Bottom", new Rect(30f, 60f, 0f, 0f) },
            { "Left", new Rect(0f, 30f, 0f, 0f) }
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
        public Texture2D Icon;
        public Rect Position;
        public Dictionary<string, EdgeNode> EdgeNodes = new Dictionary<string, EdgeNode>()
        {
            { "Top", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Right", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Bottom", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
            { "Left", new EdgeNode(new Rect(0f, 0f, 10f, 10f)) },
        };
        public IEnumerable<CommBlockNode> AdjacentBlocks;
        private bool blockSelected = false;
        private bool edgeNodeHovered = false;
        private bool edgeNodeSelected = false;

        public CommBlockNode(nodeFace nf, Rect pos)
        {
            Face = nf;
            Position = pos;
            Icon = GameDatabase.Instance.GetTexture("CommEngr/Textures/Icon", false);

            UpdateEdgeNodes();
        }

        public void DrawBlockNode()
        {
            // Draggable Block Node
            if (Position.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && !(edgeNodeSelected || edgeNodeHovered))
            {
                blockSelected = true;
            }

            if (blockSelected && Event.current.type == EventType.MouseUp)
            {
                blockSelected = false;
            }

            if (blockSelected)
            {
                Position.x = Mouse.screenPos.x - Position.width / 2;
                Position.y = Mouse.screenPos.y - Position.height / 2;
                UpdateEdgeNodes();
            }

            // Render Block Node, followed by its connectors
            if (GUI.Button(Position, Icon, HighLogic.Skin.window))
            {
                CommEngrUtils.Log(String.Format("Block Node with Rect({0}, {1}, {2}, {3}) clicked!",
                    Position.x, Position.y, Position.width, Position.height));
            }
            
            // Selectable Edge Nodes
            foreach (var edgeNode in EdgeNodes)
            {
                if (edgeNode.Value.Position.Contains(Event.current.mousePosition))
                    edgeNodeHovered = true;
                else
                    edgeNodeHovered = false;

                if (edgeNodeHovered && Event.current.type == EventType.MouseDown)
                    edgeNodeSelected = true;

                if (edgeNodeSelected && Event.current.type == EventType.MouseDown)
                {
                    edgeNodeHovered = false;
                    edgeNodeSelected = false;
                }

                // Render Edge Nodes
                if (GUI.Button(edgeNode.Value.Position, "EN"))
                {
                    CommEngrUtils.Log(String.Format("Edge Node with Rect({0}, {1}, {2}, {3}) clicked!",
                        edgeNode.Value.Position.x, edgeNode.Value.Position.y,
                        edgeNode.Value.Position.width, edgeNode.Value.Position.height));
                }
            }
        }

        private void UpdateEdgeNodes()
        {
            foreach (var edgeNode in EdgeNodes)
            {
                EdgeNodes[edgeNode.Key].Position = new Rect(
                    Position.x + EdgeNode.VertexDistance[edgeNode.Key].x - edgeNode.Value.Position.width / 2,
                    Position.y + EdgeNode.VertexDistance[edgeNode.Key].y - edgeNode.Value.Position.height / 2,
                    EdgeNodes[edgeNode.Key].Position.width,
                    EdgeNodes[edgeNode.Key].Position.height
                );
            }
        }


    }
}
