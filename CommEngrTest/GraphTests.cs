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

using KSPCommEngr;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommEngrTest
{
    [TestClass]
    public class GraphTests
    {
        private int[,] grid;
        private Graph graph;

        [TestInitialize]
        public void Setup()
        {
            grid = new int[,] {
                { 1, 1, 0, 1, 0 },
                { 0, 1, 0, 1, 1 },
                { 1, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 1, 1, 1, 0, 0 }
            };
            graph = new Graph(grid);
        }

        [TestMethod]
        public void Distance()
        {
            Node start = graph.nodes[5];
            Node goal = graph.nodes[graph.nodes.Length - 1];
            var distance = graph.Distance(start, goal);
            int expectedDistance = 7;
            Assert.AreEqual(expectedDistance, graph.Distance(start, goal));
        }
    }
}
