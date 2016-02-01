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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;

namespace KSPCommEngr
{
    public class CommConnector
    {
        public static Dictionary<string, Rect> VertexDistance = new Dictionary<string, Rect>()
        {
            { "Top", new Rect(30f, 0f, 0f, 0f) },
            { "Right", new Rect(60f, 30f, 0f, 0f) },
            { "Bottom", new Rect(30f, 60f, 0f, 0f) },
            { "Left", new Rect(0f, 30f, 0f, 0f) }
        };

        public CommBlock LConnectedNode = null;
        public CommBlock RConnectedNode = null;
        public Rect[] path;

        public Rect Position { get; set; }
        public CommConnector(Rect pos, CommBlock lNode = null, CommBlock rNode = null)
        {
            Position = pos;
            LConnectedNode = lNode;
            RConnectedNode = rNode;
            if (LConnectedNode != null && RConnectedNode != null)
                DrawConnection();
        }

        public void createConnection(CommBlock lNode, CommBlock rNode)
        {
            LConnectedNode = lNode;
            RConnectedNode = rNode;
        }

        private void DrawConnection()
        {
            foreach (var rect in path)
            {
                GLUtils.DrawConnection(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), Color.red, 3f);
            }
        }
    }
}
