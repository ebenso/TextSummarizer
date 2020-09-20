using System;
using System.Collections.Generic;

namespace TextRank
{
    public static class ExtractKeyPhrases
    {
        public static Tuple<string, List<string>> KeyPhrases(this string sentence, int wordLength = 100)
        {
            var keyWords = ExtractKeyword.Extract.GetKeyWordsList(sentence);
            var summary = ExtractSummary.Extract.ExtractParagraphSummary(sentence, wordLength);
            return new Tuple<string, List<string>>(summary, keyWords);
        }
    }
}


