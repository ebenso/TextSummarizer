using System.Collections.Generic;

namespace PageRank.Graph
{
    /// <summary>
    /// Node for Graph
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphNode<T> : Node<T>
    {
        private Neighbors<T> _neighbors;

        public GraphNode() : base()
        {
        }

        public GraphNode(T value) : base(value)
        {
        }

        public GraphNode(T value, NodeSet<T> neighbors) : base(value, neighbors)
        {
        }

        public double this[GraphNode<T> destination] => GetWeight(destination);

        /// <summary>
        /// Return the weight of Neighbor
        /// </summary>
        /// <param name="destination"></param>
        /// <returns>weight(dobule)</returns>
        private double GetWeight(GraphNode<T> destination)
        {
            Neighbor<T> nab = _neighbors.FindByValue(destination);
            if (nab == null)
                return -1;

            return nab.Weight;
        }

        public int OutDegree
        {
            get
            {
                return Neighbors.Count;
            }
        }

        /// <summary>
        /// Return Neighbors
        /// </summary>
        public new Neighbors<T> Neighbors
        {
            get
            {
                if (this._neighbors == null)
                    _neighbors = new Neighbors<T>();

                return this._neighbors;
            }
        }

        /// <summary>
        /// Return the list of Neighbors
        /// </summary>
        public List<GraphNode<T>> NeighborList
        {
            get
            {
                if (this._neighbors == null)
                    _neighbors = new Neighbors<T>();

                List<GraphNode<T>> list = new List<GraphNode<T>>();
                foreach (Neighbor<T> nab in _neighbors)
                {
                    list.Add(nab.GraphNode);
                }

                return list;
            }
        }

        /// <summary>
        /// Get a clone (Deep Copy) using refelection.
        /// </summary>
        //public GraphNode<T> Clone()
        //{
        //    //return new DeepCloneHelper(this).Clone<GraphNode<T>>();
        //    //return Utils.Clone(this) as GraphNode<T>;
        //    //return new FastDeepCloner(this).Clone<GraphNode<T>>();
        //}
    }
}