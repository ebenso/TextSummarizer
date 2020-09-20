using System;
using OpenNLP.Tools.SentenceDetect;

namespace TextRank.POSTagger
{
    internal static class SentencePOSTagger
    {
        private static string _modelPath = AppDomain.CurrentDomain.BaseDirectory + "/Resources/Models/";
        private static EnglishMaximumEntropySentenceDetector _sentence_tokenizer = new EnglishMaximumEntropySentenceDetector(_modelPath + "/EnglishSD.nbin");

        public static string[] GetTaggedSentences(string paragraph)
        {
            return _sentence_tokenizer.SentenceDetect(paragraph.Trim());
        }
    }
}
