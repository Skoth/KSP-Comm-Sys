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

namespace KSPCommEngr
{
    public class Search
    {
        public Graph graph;
        public List<Node> reachable;
        public List<Node> explored;
        public List<Node> path;
        public Node goalNode;
        public int iterations;
        public bool finished;

        public Search(Graph g)
        {
            graph = g;
        }

        public void Start(Node start, Node goal)
        {
            // Clear and reset all values
            reachable = new List<Node>();
            reachable.Add(start);

            goalNode = goal;

            explored = new List<Node>();
            path = new List<Node>();
            iterations = 0;

            for (int i = 0; i < graph.nodes.Length; ++i)
            {
                graph.nodes[i].Clear();
            }
        }

        public void Step()
        {
            if (path.Count > 0) return;

            if(reachable.Count == 0)
            {
                finished = true;
                return;
            }

            iterations++;

            // Get node
            var node = ChooseNode();

            // Check if node is goal, if it is, fill out path with previous nodes
            if(node == goalNode)
            {
                while(node != null)
                {
                    path.Insert(0, node);
                    node = node.previous;
                }
                finished = true;
                return;
            }

            reachable.Remove(node);
            explored.Add(node);

            for(var i = 0; i < node.adjacencyList.Count; ++i)
            {
                AddAdjacent(node, node.adjacencyList[i]);
            }
        }

        public void AddAdjacent(Node node, Node adjacent)
        {
            // Populate next possible solutions for Step() with non-goal node
            if(FindNode(adjacent, explored) || FindNode(adjacent, reachable))
            {
                return;
            }

            adjacent.previous = node;
            reachable.Add(adjacent);
        }

        public Node ChooseNode()
        {
            return reachable.OrderBy(
                node => graph.Distance(node, goalNode)
            ).First();
        }

        public bool FindNode(Node node, List<Node> list)
        {
            return GetNodeIndex(node, list) >= 0;
        }

        public int GetNodeIndex(Node node, List<Node> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if(node == list[i])
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
