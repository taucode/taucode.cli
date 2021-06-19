using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Lab.Data.Graphs;
using TauCode.Lab.Data.Graphs.Rho;
using TauCode.Lab.Data.Graphs.Rho.Impl;

namespace TauCode.Lab.Algorithms.Graphs.Rho
{
    public class RhoGraphSlicingAlgorithm : IAlgorithm<IRhoGraph, IReadOnlyList<IRhoGraph>>
    {
        private List<IRhoGraph> _result;

        private IRhoGraph[] Slice()
        {
            if (this.Input == null)
            {
                throw new InvalidOperationException($"'{nameof(Input)}' is null.");
            }

            _result = new List<IRhoGraph>();

            while (true)
            {
                var nodes = this.GetTopLevelNodes();
                if (nodes.Count == 0)
                {
                    if (this.Input.Nodes.Any())
                    {
                        _result.Add(this.Input);
                    }

                    break;
                }

                var slice = new RhoGraph();
                slice.CaptureNodesFrom(this.Input, nodes);
                _result.Add(slice);
            }

            return _result.ToArray();
        }

        private IReadOnlyList<IRhoNode> GetTopLevelNodes()
        {
            var result = new List<IRhoNode>();

            var nodes = this.Input.Nodes;
            foreach (var node in nodes)
            {
                var outgoingEdges = node.GetOutgoingEdgesLyingInGraph(this.Input);

                var isTopLevel = true;

                foreach (var outgoingEdge in outgoingEdges)
                {
                    if (outgoingEdge.To == node)
                    {
                        // node referencing self, don't count - it still might be "top-level"
                        continue;
                    }

                    // node referencing another node, i.e. is not "top-level"
                    isTopLevel = false;
                    break;
                }

                if (isTopLevel)
                {
                    result.Add(node);
                }
            }

            return result;
        }

        public void Run()
        {
            var list = this.Slice();
            this.Output = list.ToArray();
        }

        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            this.Run();
            return Task.CompletedTask;
        }

        public IRhoGraph Input { get; set; }

        public IReadOnlyList<IRhoGraph> Output { get; private set; }
    }
}
