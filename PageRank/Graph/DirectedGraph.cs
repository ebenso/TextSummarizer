using System;

namespace PageRank.Graph
{
    public class DirectedGraph<T> : Graph<T>
    {
        public override bool IsDirected
        {
            get
            {
                return true;
            }
        }

        public DirectedGraph() : base()
        { }

        public DirectedGraph(NodeSet<T> nodeSet) : base(nodeSet)
        { }

        public void AddEdge(T source, T destination, double weight = 1.0)
        {
            base.AddEdges(source, destination, weight);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="soruce"></param>
        /// <param name="destination"></param>
        /// <param name="weight"></param>
        public void AddEdge(GraphNode<T> soruce, GraphNode<T> destination, double weight = 1.0)
        {
            base.AddEdges(soruce, destination, weight);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="weight"></param>
        public void AddEdges(Tuple<GraphNode<T>, GraphNode<T>>[] edges, double weight = 1.0)
        {
            foreach (Tuple<GraphNode<T>, GraphNode<T>> edge in edges)
            {
                base.AddEdges(edge.Item1, edge.Item2, weight);
            }
        }
    }
}