using System;

namespace PageRank.Graph
{
    public class UnDirectedGraph<T> : Graph<T>
    {
        public override bool IsDirected => false;

        public UnDirectedGraph()
        { }

        public UnDirectedGraph(NodeSet<T> nodeSet) : base(nodeSet)
        { }

        public void AddEdge(T source, T destination, double weight = 1.0)
        {
            base.AddEdges(source, destination, weight);
            base.AddEdges(destination, source , weight);
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
            base.AddEdges(destination, soruce, weight);
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
                base.AddEdges(edge.Item2, edge.Item1, weight);
            }
        }
    }
}