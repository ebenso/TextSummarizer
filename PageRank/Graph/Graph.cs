using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PageRank.Graph
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Graph<T> : IEnumerable<GraphNode<T>>
    {
        protected NodeSet<T> _nodeSet;
        protected LookUp<T> _lookupTable;
        protected int _capacity = Int32.MaxValue;

        protected Graph()
        {
            _lookupTable = new LookUp<T>();
            _nodeSet = new NodeSet<T>();
            _capacity = Int32.MaxValue;
        }

        internal Graph(NodeSet<T> nodeSet) : this()
        {
            // add all node to lookup list
            foreach (var node in _nodeSet)
            {
                if (node != null)
                {
                    _lookupTable.Add(node.Value, node);
                    _nodeSet.Add(node);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public double this[GraphNode<T> source, GraphNode<T> destination] => GetWeight(source, destination);

        private double GetWeight(GraphNode<T> source, GraphNode<T> destination)
        {
            if (!(_nodeSet.Contains(source) && _nodeSet.Contains(destination)))
                return -1;

            //weight = Graph[Source][destination]
            double nabs = source[destination];
            return nabs;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Neighbors<T> this[GraphNode<T> source] => GetNeighbors(source);

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private Neighbors<T> GetNeighbors(GraphNode<T> source)
        {
            return source.Neighbors;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal bool Remove(T node)
        {
            //check the node in lookup table
            if (node == null && !_lookupTable.Contains(node))
                return false;

            // first remove the node from the nodeset
            var nodeToRemove = _nodeSet.Get(node);

            // node wasn't found
            if (nodeToRemove == null)
                return false;

            var danglingNeighbors = nodeToRemove.Neighbors.Where(nab => nab.Weight == 0).ToDictionary(nab => nab.GraphNode);

            // otherwise, the node was found
            bool status = _nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            // if its neighbor is dangling node then
            // we remove the neighbor but if its neighbor is not dengling then then preserve it in graph.
            // if we not want forest.

            return status;
        }

        /// <summary>
        ///
        /// </summary>
        internal List<GraphNode<T>> DanglingNodes
        {
            get
            {
                List<GraphNode<T>> _danglinNodes = new List<GraphNode<T>>();
                foreach (var node in this.OutDegree)
                {
                    if (node.Value == 0)
                        _danglinNodes.Add(node.Key);
                }

                return _danglinNodes;
            }
        }

        internal bool Contains(T value)
        {
            return _nodeSet.FindByValue(value) != null;
        }

        public IEnumerator<GraphNode<T>> GetEnumerator()
        {
            return (IEnumerator<GraphNode<T>>)_nodeSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<GraphNode<T>> Nodes
        {
            get
            {
                return _nodeSet.ToList;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.Count > 0 ? false : true;
            }
        }

        public int Count
        {
            get { return _nodeSet.Count; }
        }

        public Dictionary<GraphNode<T>, double> OutDegree
        {
            get
            {
                if (this.IsEmpty)
                    return null;
                else
                    return GetOutDegree();
            }
        }

        public abstract bool IsDirected { get; }

        private Dictionary<GraphNode<T>, double> GetOutDegree()
        {
            Dictionary<GraphNode<T>, double> outDegree = new Dictionary<GraphNode<T>, double>();

            //build graph outdegree dictionary
            foreach (GraphNode<T> node in this)
                outDegree.Add(node, node.OutDegree);

            return outDegree;
        }

        internal GraphNode<T> FindByValue(T value)
        {
            GraphNode<T> node = null;
            node = this._nodeSet.FindByValue(value);

            return node;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected GraphNode<T> Add(T value)
        {
            if (value == null)
                return null;
            try
            {
                //first check the value in _lookup table
                if (_lookupTable.Contains(value))
                {
                    var lookup = _lookupTable.Get(value);
                    return lookup;
                }

                GraphNode<T> node = new GraphNode<T>();
                node.Value = value;
                _lookupTable.Add(value, node);
                _nodeSet.Add(node);
                return node;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="weight"></param>
        protected void AddEdges(GraphNode<T> source, GraphNode<T> destination, double weight = 1.0)
        {
            if (source == null || destination == null)
                return;

            var lookup = _lookupTable.Get(source.Value);

            if (lookup == null || !(source == lookup))
            {
                _lookupTable.Add(source.Value, source);
                _nodeSet.Add(source);
            }
            if (!(destination == _lookupTable.Get(destination.Value)))
            {
                _lookupTable.Add(destination.Value, destination);
                _nodeSet.Add(destination);
            }

            source.Neighbors.Add(destination as GraphNode<T>, weight);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="weight"></param>
        protected void AddEdges(T source, T destination, double weight = 1.0)
        {
            if (source == null || destination == null)
                return;

            var gsource = this.Add(source);
            var gdestination = this.Add(destination);

            this.AddEdges(gsource, gdestination, weight);
        }
    }
}