#region license

/* The MIT License (MIT)

 * Copyright (c) 2016 Skoth

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
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using KSP;
using KSPCommEngr.Extensions;
using KSPCommEngr;

namespace KSPCommEngr
{
    public enum nodeFace
    {
        BandpassFilter,
        DataSource,
        DataStore,
        HighpassFilter,
        LocalOscillator,
        LowpassFilter,
        Multiplier,
        Receiver,
        Transmitter
    }

    // Non-GUI ops specified in IVertex interface
    public class CommBlock : ITransform
    {
        public nodeFace Face { get; set; }
        public Texture2D Icon;
        public Rect Position;
        public Dictionary<string, CommConnector> EdgeNodes = new Dictionary<string, CommConnector>()
        {
            { "Top", new CommConnector(new Rect(0f, 0f, 10f, 10f)) },
            { "Right", new CommConnector(new Rect(0f, 0f, 10f, 10f)) },
            { "Bottom", new CommConnector(new Rect(0f, 0f, 10f, 10f)) },
            { "Left", new CommConnector(new Rect(0f, 0f, 10f, 10f)) },
        };
        public IEnumerable<CommBlock> AdjacentBlocks;
        private Vector2? offsetPos = null;
        private bool blockSelected = false;
        private bool edgeNodeHovered = false;
        private bool edgeNodeSelected = false;
        private int guiDepth = 2;
        private static Vector2 screenPt1;
        private bool callDrawEdge;

        public Func<float, float> TransferFunction;

        public CommBlock(nodeFace nf, Rect pos, Func<float, float> expression = null)
        {
            Face = nf;
            Position = pos;
            Icon = GameDatabase.Instance.GetTexture(String.Format("CommEngr/Textures/{0}", Face.ToString()), false);

            // TODO: Formulate lambda-friendly DSP functional implementation
            TransferFunction = expression == null ? (x) => 1 : expression;

            UpdateEdgeNodes();
        }

        private static GUIStyle GetBlockStyle(Texture2D tex)
        {
            return new GUIStyle()
            {
                active = HighLogic.Skin.button.active,
                alignment = TextAnchor.MiddleCenter,
                border = new RectOffset(0, 0, 0, 0),
                clipping = TextClipping.Clip,
                contentOffset = new Vector2(0, 0),
                fixedHeight = tex.height,
                fixedWidth = tex.width,
                focused = HighLogic.Skin.window.focused,
                font = null,
                hover = HighLogic.Skin.button.hover,
                imagePosition = ImagePosition.ImageOnly,
                margin = new RectOffset(0, 0, 0, 0),
                name = tex.name,
                normal = HighLogic.Skin.button.normal,
                onActive = new GUIStyleState() { background = HighLogic.Skin.button.active.background, textColor = Color.white },
                onFocused = new GUIStyleState() { background = HighLogic.Skin.button.focused.background, textColor = Color.white },
                onHover = new GUIStyleState() { background = HighLogic.Skin.button.hover.background, textColor = Color.white },
                onNormal = new GUIStyleState() { background = HighLogic.Skin.button.normal.background, textColor = Color.white },
                overflow = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                richText = false,
                stretchHeight = false,
                stretchWidth = false,
                wordWrap = true
            };
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
                offsetPos = null;
            }

            if (blockSelected)
            {
                guiDepth = 1;

                // Enables drag and drop from any section of the block clicked
                if (offsetPos == null)
                    offsetPos = new Vector2(Mouse.screenPos.x - Position.x, Mouse.screenPos.y - Position.y);

                // Utilize grid class for quantizing movement
                Position.x = Grid.SnapToGrid(Mouse.screenPos.x - offsetPos.Value.x);
                Position.y = Grid.SnapToGrid(Mouse.screenPos.y - offsetPos.Value.y);
                UpdateEdgeNodes();
            }
            else guiDepth = 2;

            // Render Block Node, followed by its connectors
            GUI.depth = guiDepth;
            if (GUI.Button(Position, Icon, GetBlockStyle(Icon)))
            {
                CommEngrUtils.Log(string.Format("Block with Rect({0}, {1}, {2}, {3}) clicked!",
                    Position.x, Position.y, Position.width, Position.height));
            }

            // Selectable Edge Nodes
            GUI.depth = 0;
            foreach (var edgeNode in EdgeNodes)
            {
                if (edgeNode.Value.Position.Contains(Event.current.mousePosition))
                    edgeNodeHovered = true;
                else
                    edgeNodeHovered = false;

                if (edgeNodeHovered && Input.GetMouseButtonDown(0))
                {
                    screenPt1 = new Vector2(edgeNode.Value.Position.x, (float)Screen.height - edgeNode.Value.Position.y);
                    edgeNodeSelected = true;
                    callDrawEdge = true;
                }

                if (edgeNodeSelected && Input.GetMouseButtonDown(1))
                {
                    callDrawEdge = false;
                    edgeNodeHovered = false;
                    edgeNodeSelected = false;
                }
                if (GUI.Button(edgeNode.Value.Position, "EN"))
                {

                }

                if (edgeNodeSelected)
                {

                }

                if (callDrawEdge)
                {
                    GLUtils.DrawConnection(screenPt1 + new Vector2(edgeNode.Value.Position.width / 2f, edgeNode.Value.Position.height / 2f), Mouse.screenPos, Color.red, 10f);
                }
            }
        }

        private void UpdateEdgeNodes()
        {
            foreach (var edgeNode in EdgeNodes)
            {
                EdgeNodes[edgeNode.Key].Position = new Rect(
                    Position.x + CommConnector.VertexDistance[edgeNode.Key].x - edgeNode.Value.Position.width / 2,
                    Position.y + CommConnector.VertexDistance[edgeNode.Key].y - edgeNode.Value.Position.height / 2,
                    EdgeNodes[edgeNode.Key].Position.width,
                    EdgeNodes[edgeNode.Key].Position.height
                );
            }
        }

        Signal ITransform.TransferFunction(params Signal[] input)
        {
            return TransferFunction(input);
        }
    }
}
