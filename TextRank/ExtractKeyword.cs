using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageRank.Graph;
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
            var taggedList = WordPOSTagger.GetPosTaggedTokens(sentence);
            var directedGraph = GraphUtil.GraphInstance.BuildPOSGraph<string>(taggedList);
            var rank = new PageRank<string>();
            var rankedDictionary = rank.Rank(directedGraph);

            var word_list = ExtractUtil.instance.GetNormalizedUniqueWordList(taggedList);

            var keywords = rankedDictionary?.OrderByDescending(p => p.Value).Take(rankedDictionary.Count/3).Select(p => p.Key).ToList();
            IList<string> joinedKeywords = null;
            if (keywords != null)
                joinedKeywords= ExtractUtil.instance.JoinAdjacentWords(word_list, keywords);
            return joinedKeywords as List<string>;

            
            
        }
    }
}
