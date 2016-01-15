using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KSPCommEngr;
using System.Collections.Generic;

namespace CommEngrTest
{
    [TestClass]
    public class NodeTests
    {
        private Graph graph;
        private Node node;

        [TestInitialize]
        public void Setup()
        {
            graph = new Graph(new int[,] {
                { 0, 0, 0 },
                { 0, 0, 1 }, // Obstacle still part of adjacencyList
                { 0, 0, 0 }
            });
            node = graph.nodes[graph.nodes.Length / 2];
        }

        [TestMethod]
        public void Neighbors()
        {
            int expectedLength = 4;
            Assert.AreEqual(expectedLength, node.adjacencyList.Count);

            List<Node> expectedNeighbors = new List<Node>();
            expectedNeighbors.Add(graph.nodes[1]);
            expectedNeighbors.Add(graph.nodes[3]);
            expectedNeighbors.Add(graph.nodes[5]);
            expectedNeighbors.Add(graph.nodes[7]);
            foreach (var neighbor in node.adjacencyList)
            {
                CollectionAssert.Contains(expectedNeighbors, neighbor);
            }
        }
    }
}
