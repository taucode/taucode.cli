using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Lab.Data.Graphs;

// todo clean
namespace TauCode.Lab.Algorithms.Graphs
{
    public class GraphSlicingAlgorithm<T> : IAlgorithm<IGraph<T>, IReadOnlyList<IGraph<T>>>
    {
        private List<IGraph<T>> _result;

        //private readonly IGraph<T> _graph;
        //private List<IGraph<T>> _result;

        //public GraphSlicingAlgorithm(/*IGraph<T> graph*/)
        //{
        //    //_graph = graph ?? throw new ArgumentNullException(nameof(graph));
        //}

        private IGraph<T>[] Slice()
        {
            if (this.Input == null)
            {
                throw new NotImplementedException();
            }

            _result = new List<IGraph<T>>();

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

                var slice = new Graph<T>();
                slice.CaptureNodesFrom(this.Input, nodes);
                _result.Add(slice);
            }

            return _result.ToArray();
        }

        private IReadOnlyList<INode<T>> GetTopLevelNodes()
        {
            var result = new List<INode<T>>();

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

        public IGraph<T> Input { get; set; }
        public IReadOnlyList<IGraph<T>> Output { get; private set; }
    }
}
