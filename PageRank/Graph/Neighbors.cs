using System;
using System.Collections.ObjectModel;

namespace PageRank.Graph
{
    /// <summary>
    /// Store List of Neighbor for each GraphNode
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Neighbors<T> : Collection<Neighbor<T>>
    {
        public Neighbors() : base()
        {
        }

        public Neighbors(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Neighbor<T>));
        }

        public Neighbor<T> Find(Neighbor<T> value)
        {
            // search the list for the value
            foreach (Neighbor<T> node in Items)
                if (node.GraphNode.Equals(value.GraphNode))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }

        public Neighbor<T> FindByValue(GraphNode<T> value)
        {
            // search the list for the value
            foreach (Neighbor<T> node in Items)
                if (node.GraphNode.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }

        public bool Add(Neighbor<T> neighbor)
        {
            try
            {
                if (Find(neighbor) == null)
                    base.Items.Add(neighbor);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public void Add(GraphNode<T> destination, double weight)
        {
            foreach(var nbr in this)
            {
                if(nbr.GraphNode == destination)
                {
                    nbr.setWeight(weight);
                    return;
                }
            }

            Neighbor<T> nab = new Neighbor<T>(destination, weight);
            this.Add(nab);
        }
    }
}