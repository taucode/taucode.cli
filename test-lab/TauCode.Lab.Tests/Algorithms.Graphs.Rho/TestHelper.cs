using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Lab.Data.Graphs.Rho;
using TauCode.Lab.Data.Graphs.Rho.Impl;

namespace TauCode.Lab.Tests.Algorithms.Graphs.Rho
{
    internal static class TestHelper
    {
        internal static IRhoEdge[] LinkTo(this IRhoNode node, params IRhoNode[] otherNodes)
        {
            return otherNodes
                .Select(node.DrawEdgeTo)
                .ToArray();
        }

        internal static IRhoNode GetNode(this IRhoGraph graph, string nodeValue)
        {
            return graph.Nodes.Single(x => x.Name == nodeValue);
        }

        internal static void AssertNode(
            this IRhoGraph graph,
            IRhoNode node,
            IRhoNode[] linkedToNodes,
            IRhoEdge[] linkedToEdges,
            IRhoNode[] linkedFromNodes,
            IRhoEdge[] linkedFromEdges)
        {
            if (linkedToNodes.Length != linkedToEdges.Length)
            {
                throw new ArgumentException();
            }

            if (linkedFromNodes.Length != linkedFromEdges.Length)
            {
                throw new ArgumentException();
            }

            Assert.That(graph.ContainsNode(node), Is.True);

            // check 'outgoing' edges
            Assert.That(node.GetOutgoingEdgesLyingInGraph(graph).Count, Is.EqualTo(linkedToNodes.Length));

            foreach (var outgoingEdge in node.GetOutgoingEdgesLyingInGraph(graph))
            {
                Assert.That(outgoingEdge.From, Is.EqualTo(node));

                var to = outgoingEdge.To;
                Assert.That(graph.ContainsNode(to), Is.True);
                Assert.That(to.IncomingEdges, Does.Contain(outgoingEdge));

                var index = Array.IndexOf(linkedToNodes, to);
                Assert.That(index, Is.GreaterThanOrEqualTo(0));
                Assert.That(outgoingEdge, Is.SameAs(linkedToEdges[index]));
            }

            // check 'incoming' edges
            Assert.That(node.GetIncomingEdgesLyingInGraph(graph).Count, Is.EqualTo(linkedFromNodes.Length));

            foreach (var incomingEdge in node.GetIncomingEdgesLyingInGraph(graph))
            {
                Assert.That(incomingEdge.To, Is.EqualTo(node));

                var from = incomingEdge.From;
                Assert.That(graph.ContainsNode(from), Is.True);
                Assert.That(from.OutgoingEdges, Does.Contain(incomingEdge));

                var index = Array.IndexOf(linkedFromNodes, from);
                Assert.That(index, Is.GreaterThanOrEqualTo(0));
                Assert.That(incomingEdge, Is.SameAs(linkedFromEdges[index]));
            }
        }

        internal static IRhoNode AddNamedNode(this IRhoGraph graph, string name)
        {
            var node = new RhoNode
            {
                Name = name,
            };

            graph.AddNode(node);
            return node;
        }
    }
}
