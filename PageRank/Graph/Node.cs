namespace PageRank.Graph
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Node<T>
    {
        // Private member-variables
        private T _data;

        private NodeSet<T> _neighbors;
        public Node()
        {
        }

        public Node(T data) : this(data, null)
        {
        }

        public Node(T data, NodeSet<T> neighbors)
        {
            this._data = data;
            this._neighbors = neighbors;
        }

        public T Value
        {
            get => _data;
            set => _data = value;
        }

        protected NodeSet<T> Neighbors
        {
            get => _neighbors;
            set => _neighbors = value;
        }

        public override string ToString()
        {
            return _data.ToString();
        }
    }
}