using System;
using System.Collections.Generic;

namespace PageRnak.Graph
{
    /// <summary>
    /// Lookup table to idendify the comming value for node is
    /// already exist, if exist then it return,
    /// otherwise it would be create a GraphNode and then return.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LookUp<T>
    {
        private Dictionary<T, GraphNode<T>> lookupSet;

        public LookUp()
        {
            lookupSet = new Dictionary<T, GraphNode<T>>();
        }

        public GraphNode<T> Get(T key)
        {
            GraphNode<T> value = null;
            try
            {
                if (lookupSet.ContainsKey(key))
                    lookupSet.TryGetValue(key, out value);
            }
            catch (Exception)
            {
                throw;
            }

            return value;
        }

        public bool Contains(T key)
        {
            return lookupSet.ContainsKey(key);
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

                lookupSet.Add(key, value);
            }
            catch (Exception)
            {
                throw;
            }

            return status;
        }
    }
}