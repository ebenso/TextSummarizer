using System.Linq;

namespace PageRnak.Graph
{
    public class GraphUtils
    {
        public static DirectedGraph<T> ToStochastic<T>(Graph<T> graph)
        {
            DirectedGraph<T> sgrapah = new DirectedGraph<T>();

            var nodeOutDegree = graph.OutDegree;

            //calculate the outdegree weight sum
            var weightedDegree = nodeOutDegree.ToDictionary(item => item.Key, item => item.Key.Neighbors.Sum(nab => nab.Weight));

            foreach (var nodewithweight in weightedDegree)
            {
                GraphNode<T> source = nodewithweight.Key;
                double outdegree = nodewithweight.Value;

                foreach (var nab in source.Neighbors)
                {
                    GraphNode<T> target = nab.GraphNode;
                    double weight = nab.Weight <= 0.0 ? 1 : nab.Weight;

                    //Create new node and add this to graph
                    sgrapah.AddEdge(source.Value, target.Value, weight / outdegree);
                }
            }
            return sgrapah;
        }
    }
}