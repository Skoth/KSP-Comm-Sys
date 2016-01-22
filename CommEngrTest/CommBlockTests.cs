using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KSPCommEngr;

namespace CommEngrTest
{
    [TestClass]
    public class CommBlockTests
    {
        [TestMethod]
        public void UpdateGraphOnDragStop()
        {
            Graph graph = new Graph(new int[,] {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            });
            Graph expectedGraph = new Graph(new int[,] {
                { 0, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0 }
            });

            // User Drag and Drop Event

            CollectionAssert.AreEqual(expectedGraph.nodes, graph.nodes);
        }
    }
}
