using System;

namespace PageRank.Graph
{
    /// <summary>
    /// Store informatioin about neighbor and its weight
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Neighbor<T>
    {
        private readonly GraphNode<T> _neighbor;
        private double _weight;

        public Neighbor(GraphNode<T> neighbor, double weight)
        {
            this._neighbor = neighbor;
            this._weight = weight;
        }

        public GraphNode<T> GraphNode
        {
            get
            {
                return _neighbor;
            }
        }

        public double Weight { get => _weight; private set => _weight = value; }

        internal void setWeight(double weight)
        {
            try
            {
                this._weight = weight;
            }
            catch (Exception) { }
        }
    }
}