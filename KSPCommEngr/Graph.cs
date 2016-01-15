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
using System.Collections;

namespace KSPCommEngr
{
    // Forms node array and builds adjacency list for each node
    // Inspired by Unity 5 2D: Pathfinding lynda.com course by Jesse Freeman
    public class Graph
    {
        public int rows = 0;
        public int columns = 0;
        public Node[] nodes;

        public int Distance(Node x, Node y)
        {
            int xRow = x.Id / rows + 1;
            int yRow = y.Id / rows + 1;
            int xCol = x.Id % rows + 1;
            int yCol = y.Id % rows + 1;
            return Math.Abs(yRow - xRow) + Math.Abs(yCol - xCol);
        }

        // Grid that forms tile map
        public Graph(int[,] grid)
        {
            rows = grid.GetLength(0);
            columns = grid.GetLength(1);
            
            // Since each node is going to store a reference to its own neighbors,
            // the node data can be kept as a flattened array
            nodes = new Node[grid.Length];

            // Setup initial empty nodes
            for (int i = 0; i < nodes.Length; ++i)
            {
                var node = new Node();
                node.Id = i;
                nodes[i] = node;
            }

            // Build adjacency list for each node of grid
            for (int r = 0; r < rows; ++r)
            {
                for (int c = 0; c < columns; ++c)
                {
                    Node node = nodes[columns * r + c];

                    // 1 represents unusable node for path, 0 is an available tile
                    // If the current node in the grid is an obstacle, check if corner
                    // If not, a crossing formed by parallel neighbors can be created
                    if (grid[r, c] == 1)
                    {
                        if (r > 0 && r < rows - 1 && c > 0 && c < columns - 1)
                        {
                            if (grid[r - 1, c] == 1 && grid[r + 1, c] == 1 &&
                                grid[r, c - 1] == 0 && grid[r, c + 1] == 0)
                            {
                                node.adjacencyList.Add(nodes[columns * r + c + 1]);
                                node.adjacencyList.Add(nodes[columns * r + c - 1]);
                            }
                            else if (grid[r - 1, c] == 0 && grid[r + 1, c] == 0 &&
                                grid[r, c - 1] == 1 && grid[r, c + 1] == 1)
                            {
                                node.adjacencyList.Add(nodes[columns * (r - 1) + c]);
                                node.adjacencyList.Add(nodes[columns * (r + 1) + c]);
                            }
                        }
                        continue;
                    }

                    // Up
                    if (r > 0)
                    {
                        node.adjacencyList.Add(nodes[columns * (r - 1) + c]);
                    }

                    // Right
                    if (c < columns - 1)
                    {
                        node.adjacencyList.Add(nodes[columns * r + c + 1]);
                    }

                    // Down
                    if (r < rows - 1)
                    {
                        node.adjacencyList.Add(nodes[columns * (r + 1) + c]);
                    }

                    // Left
                    if (c > 0)
                    {
                        node.adjacencyList.Add(nodes[columns * r + c - 1]);
                    }
                }
            }
        }
    }
}
