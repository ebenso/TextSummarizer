using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PageRank.Graph
{
    public class NodeSet<T> : IEnumerable<GraphNode<T>>
    {
        private HashSet<GraphNode<T>> _nodeSet;

        public NodeSet()
        {
            // Add the specified number of items
            _nodeSet = new HashSet<GraphNode<T>>();
        }

        public NodeSet(List<GraphNode<T>> nodeList) : this()
        {
            try
            {
                foreach (var node in nodeList)
                    _nodeSet.Add(node);
            }
            catch (Exception ex)
            { }
        }

        public GraphNode<T> FindByValue(T value)
        {
            foreach (var node in _nodeSet)
            {
                var t = node.Value.Equals(value);
                if (t)
                    return node as GraphNode<T>;
            }

            // if we reached here, we didn't find a matching node
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public GraphNode<T> Get(T value)
        {
            return this.FindByValue(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Remove(T value)
        {
            try
            {
                var nodeToRemove = this.FindByValue(value) as GraphNode<T>;
                _nodeSet.RemoveWhere(node => node == nodeToRemove);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nodeToBeRemoved"></param>
        /// <returns></returns>
        public bool Remove(GraphNode<T> nodeToBeRemoved)
        {
            try
            {
                _nodeSet.RemoveWhere(node => node == nodeToBeRemoved);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        internal void Add(GraphNode<T> node)
        {
            try
            {
                _nodeSet.Add(node);
            }
            catch (Exception) { };
        }

        public IEnumerator<GraphNode<T>> GetEnumerator()
        {
            return _nodeSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        new public List<GraphNode<T>> ToList
        {
            get
            {
                if (_nodeSet == null)
                    _nodeSet = new HashSet<GraphNode<T>>();

                return _nodeSet.ToList();
            }
        }

        public int Count
        {
            get
            {
                return _nodeSet.Count > 0 ? _nodeSet.Count : 0;
            }
        }
    }
}