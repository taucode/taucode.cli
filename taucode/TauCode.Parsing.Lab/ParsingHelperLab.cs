using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Lab
{
    public static class ParsingHelperLab
    {
        public static HashSet<INode> GetNonIdleNodesLab(IReadOnlyCollection<INode> nodes)
        {
            if (nodes.Any(x => x is IdleNode))
            {
                var result = new HashSet<INode>();
                var idleNodes = new HashSet<IdleNode>();

                foreach (var node in nodes)
                {
                    if (node == null)
                    {
                        throw new ArgumentException($"'{nameof(nodes)}' must not contain nulls.");
                    }

                    WriteNonIdleNodesLab(node, result, idleNodes);
                }

                return result;
            }
            else
            {
                return new HashSet<INode>(nodes);
            }
        }

        private static void WriteNonIdleNodesLab(INode node, HashSet<INode> destination, HashSet<IdleNode> idleNodes)
        {
            if (node is IdleNode idleNode)
            {
                if (idleNodes.Contains(idleNode))
                {
                    // won't do anything.
                }
                else
                {
                    idleNodes.Add(idleNode);
                    var links = node.ResolveLinks();
                    foreach (var link in links)
                    {
                        WriteNonIdleNodesLab(link, destination, idleNodes);
                        //WriteNonIdleNodes(link, destination);
                    }
                }
            }
            else
            {
                destination.Add(node);
            }
        }
    }
}
