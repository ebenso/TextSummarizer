using System.Linq;
using System.Text;
using System.Collections.Generic;

using PageRank.Rank;
using TextRank.Helpers;
using TextRank.POSTagger;

namespace TextRank
{
    internal sealed class ExtractSummary
    {
        public static ExtractSummary Extract { get; } = new ExtractSummary();

        public string ExtractParagraphSummary(string paragraph, int wordLength =100)
        {
            var taggedSentences = SentencePOSTagger.GetTaggedSentences(paragraph);
            var directedGraph = GraphUtil.GraphInstance.BuildPOSGraph<string>(taggedSentences);
            var rankedDictionary = new PageRank<string>().Rank(directedGraph);
            var rankedSentencesList = rankedDictionary?.ToList().OrderByDescending(p => p.Value).Select(x => x.Key).ToList();
            var topSentences = new List<string>();

            if (rankedSentencesList != null)
            {
                int wordCount = 0;
                int index = 0;

                foreach (var sentences in rankedSentencesList)
                {
                    wordCount = sentences.Split(null).Length + wordCount;
                    if (wordCount >= wordLength)
                    {
                        break;
                    }
                    index++;
                }

                if (wordCount < wordLength) index--;
                topSentences = rankedSentencesList.Take(index).ToList();
            }

            StringBuilder summry = new StringBuilder("");

            foreach (var taggedSentence in taggedSentences)
            {
                foreach (var summarySentence in topSentences)
                {
                    if (summarySentence.Equals(taggedSentence))
                    {
                        summry.Append(summarySentence);
                        summry.Append("<br/><br/>");
                    }
                }
            }

            var origLength = paragraph.Length;
            var summary = summry.ToString(); 
            var summarizedLength = summry.Length;

            return summary;
        }
    }
}
