using System;
using System.Collections.Generic;

namespace TauCode.Lab.Data.Graphs
{
    public static class GraphExtensions
    {
        public static INode<T> AddNode<T>(this IGraph<T> graph, T value)
        {
            var node = new Node<T>(value);
            graph.AddNode(node);
            return node;
        }

        public static IReadOnlyCollection<IEdge<T>> GetEdges<T>(this IGraph<T> graph)
        {
            var result = new HashSet<IEdge<T>>();

            var nodes = graph.Nodes;

            foreach (var node in nodes)
            {
                var outgoingEdges = node.OutgoingEdges;
                foreach (var outgoingEdge in outgoingEdges)
                {
                    if (graph.ContainsNode(outgoingEdge.To))
                    {
                        result.Add(outgoingEdge);
                    }
                }

                var incomingEdges = node.IncomingEdges;
                foreach (var incomingEdge in incomingEdges)
                {
                    if (graph.ContainsNode(incomingEdge.From))
                    {
                        result.Add(incomingEdge);
                    }
                }
            }

            return result;
        }

        public static IGraph<T> CloneGraph<T>(this IGraph<T> graph)
        {
            var clone = new Graph<T>();
            foreach (var node in graph.Nodes)
            {
                clone.AddNode(node);
            }

            return clone;
        }

        public static IReadOnlyList<IEdge<T>> GetOutgoingEdgesLyingInGraph<T>(this INode<T> node, IGraph<T> graph)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (!graph.ContainsNode(node))
            {
                throw new InvalidOperationException("Graph does not contain this node.");
            }

            var edges = new List<IEdge<T>>();

            foreach (var outgoingEdge in node.OutgoingEdges)
            {
                var to = outgoingEdge.To;

                if (graph.ContainsNode(to))
                {
                    edges.Add(outgoingEdge);
                }
            }

            return edges;
        }

        public static IReadOnlyList<IEdge<T>> GetIncomingEdgesLyingInGraph<T>(this INode<T> node, IGraph<T> graph)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (!graph.ContainsNode(node))
            {
                throw new InvalidOperationException("Graph does not contain this node.");
            }

            var edges = new List<IEdge<T>>();

            foreach (var incomingEdge in node.IncomingEdges)
            {
                var from = incomingEdge.From;

                if (graph.ContainsNode(from))
                {
                    edges.Add(incomingEdge);
                }
            }

            return edges;
        }

        public static void CaptureNodesFrom<T>(
            this IGraph<T> graph,
            IGraph<T> otherGraph,
            IEnumerable<INode<T>> otherGraphNodes)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (otherGraph == null)
            {
                throw new ArgumentNullException(nameof(otherGraph));
            }

            if (otherGraphNodes == null)
            {
                throw new ArgumentNullException(nameof(otherGraphNodes));
            }

            var idx = 0;

            foreach (var otherGraphNode in otherGraphNodes)
            {
                if (otherGraphNode == null)
                {
                    throw new ArgumentException($"'{nameof(otherGraphNode)}' cannot contain nulls.");
                }

                if (graph.ContainsNode(otherGraphNode))
                {
                    throw new ArgumentException($"Node with index {idx} already belongs to '{nameof(graph)}'.");
                }

                var captured = otherGraph.RemoveNode(otherGraphNode);
                if (!captured)
                {
                    throw new ArgumentException($"Node with index {idx} does not belong to '{nameof(otherGraph)}'.");
                }

                graph.AddNode(otherGraphNode);

                idx++;
            }
        }
    }
}
