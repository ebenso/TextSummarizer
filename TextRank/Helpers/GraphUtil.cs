using System;
using System.Collections.Generic;
using System.Linq;
using PageRank.Graph;

namespace TextRank.Helpers
{
    internal sealed class GraphUtil
    {
        private GraphUtil(){}

        public static GraphUtil GraphInstance { get; } = new GraphUtil();

        //ref: stackoverflow.com/questions/1952153/
        private static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new[] { t2 }));
        }

        public IList<Tuple<string, string>> BuildGraph(IList<string> words)
        {
            return GetKCombs(words, 2).Select(x =>
            {
                var enumerable = x as string[] ?? x.ToArray();
                return new Tuple<string, string>(enumerable.ToList()[0], enumerable.ToList()[1]);
            }).ToList();
        }

        private static UnDirectedGraph<string> GetDirectedGraph(IList<Tuple<string,string>> uniqueKeys)
        {
            var levDistance = new LevenhteinDistance();
            IList<Tuple<string, string, int>> graph = uniqueKeys.Select(x =>
                new Tuple<string, string, int>(x.Item1, x.Item2, levDistance.Calculate(x.Item1, x.Item2))).ToList();

            var directedGraph = new UnDirectedGraph<string>();

            foreach (var node in graph)
            {
                directedGraph.AddEdge(node.Item1, node.Item2, node.Item3);
            }

            return directedGraph;
        }

        public UnDirectedGraph<string> BuildPOSGraph<T>(IList<Tuple<string,string>> taggedList)
        {
            var uniqueWords = GraphInstance.BuildGraph(ExtractUtil.instance.GetNormalizedUniqueWordList(taggedList));

            return GetDirectedGraph(uniqueWords);

        }

        public UnDirectedGraph<string> BuildPOSGraph<T>(string[] taggedSentencesList)
        {
            var sentenceGraph = GraphInstance.BuildGraph(taggedSentencesList);

            return GetDirectedGraph(sentenceGraph);
        }
    }
}
