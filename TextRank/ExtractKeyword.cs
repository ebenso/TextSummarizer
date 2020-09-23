using System.Linq;
using System.Collections.Generic;

using PageRank.Rank;
using TextRank.Helpers;
using TextRank.POSTagger;

namespace TextRank
{
    internal sealed class ExtractKeyword
    {
        public static ExtractKeyword Extract { get; } = new ExtractKeyword();

        public List<string> GetKeyWordsList(string sentence)
        {
            IList<string> joinedKeywords = null;
            var taggedList = WordPOSTagger.GetPosTaggedTokens(sentence);
            var directedGraph = GraphUtil.GraphInstance.BuildPOSGraph<string>(taggedList);
            var rank = new PageRank<string>();
            var rankedDictionary = rank.Rank(directedGraph);

            var wordList = ExtractUtil.instance.GetNormalizedUniqueWordList(taggedList);

            var keywords = rankedDictionary?.OrderByDescending(p => p.Value)
                .Take(rankedDictionary.Count/3)
                .Select(p => p.Key)
                .ToList();

            if (keywords != null)
                joinedKeywords= ExtractUtil.instance.JoinAdjacentWords(wordList, keywords);
            
            return joinedKeywords as List<string>;
        }
    }
}
