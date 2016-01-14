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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KSPCommEngr;
using System.Collections.Generic;
using System.Diagnostics;

namespace CommEngrTest
{
    [TestClass]
    public class SearchTests
    {
        public Graph graph;

        [TestInitialize]
        public void Setup()
        {
            int[,] nodes = new int[,] {
                { 1, 1, 0, 1, 0 },
                { 0, 1, 0, 1, 1 }, 
                { 1, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 1, 1, 1, 0, 0 }  
            };
            graph = new Graph(nodes);
        }

        [TestMethod]
        public void ChooseNodeTest()
        {
            Search search = new Search(graph);
            Node goal = graph.nodes[graph.nodes.Length - 1];
            search.goalNode = goal;
            List<Node> reachable = new List<Node>();
            reachable.Add(graph.nodes[15]);
            reachable.Add(graph.nodes[21]);
            search.reachable = reachable;
            Node optimalNode = graph.nodes[21];
            Assert.AreSame(search.ChooseNode(), optimalNode);

            search.goalNode = graph.nodes[0];
            optimalNode = graph.nodes[15];
            Assert.AreSame(search.ChooseNode(), optimalNode);
        }
    }
}
