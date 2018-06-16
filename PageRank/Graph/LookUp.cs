using System;
using System.Collections.Generic;

namespace PageRank.Graph
{
    /// <summary>
    /// Lookup table to idendify the comming value for node is
    /// already exist, if exist then it return,
    /// otherwise it would be create a GraphNode and then return.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LookUp<T>
    {
        private readonly Dictionary<T, GraphNode<T>> _lookupSet;

        public LookUp()
        {
            _lookupSet = new Dictionary<T, GraphNode<T>>();
        }

        public GraphNode<T> Get(T key)
        {
            GraphNode<T> value = null;
            try
            {
                if (_lookupSet.ContainsKey(key))
                    _lookupSet.TryGetValue(key, out value);
            }
            catch (Exception)
            {
                throw;
            }

            return value;
        }

        public bool Contains(T key)
        {
            return _lookupSet.ContainsKey(key);
        }

        /// <summary>
        /// Add the GraphNode in lookup table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(T key, GraphNode<T> value)
        {
            bool status = true;

            try
            {
                if ((object)key == null || value != null)
                    status = false;

                _lookupSet.Add(key, value);
            }
            catch (Exception)
            {
                throw;
            }

            return status;
        }
    }
}