using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var top_sentences = new List<string>();

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

                top_sentences = rankedSentencesList.Take(index).ToList();

            }

            StringBuilder summry = new StringBuilder("");

            foreach (var sent in taggedSentences)
            {
                foreach (var summary_sentence in top_sentences)
                {
                    if (summary_sentence.Equals(sent))
                    {
                        summry.Append(summary_sentence);
                        summry.Append("<br/><br/>");
                    }
                }
            }

            var orig_length = paragraph.Length;
            var summary = summry.ToString(); //string.Join(" ", summary_word);
            var summarized_length = summry.Length;

            return summary;
        }
    }
}
