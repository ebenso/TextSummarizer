using System;
using System.Collections.Generic;
using System.Linq;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.Tokenize;

namespace TextRank.POSTagger
{
    // ReSharper disable once InconsistentNaming
    internal static class WordPOSTagger
    {
        private static readonly EnglishRuleBasedTokenizer Tokenizer = new EnglishRuleBasedTokenizer(false);

        private static readonly string ModelPath = AppDomain.CurrentDomain.BaseDirectory + "/Resources/Models/";

        private static readonly IList<string> RequiredTags = new List<string> { "NN", "JJ", "NNP", "NNS", "NNPS" };

        private static IList<Tuple<string, string>> GetFilteredTokens(IList<Tuple<string, string>> taggedTokens)
        {
            for (int i = taggedTokens.Count - 1; i >= 0; i--)
            {
                if (RequiredTags.All(x => x != taggedTokens[i].Item2))
                {
                    taggedTokens.RemoveAt(i);
                }
            }
            return taggedTokens;
        }


        public static IList<Tuple<string, string>> GetPosTaggedTokens(string sentence)
        {
            var posTagger =
                new EnglishMaximumEntropyPosTagger(ModelPath + "/EnglishPOS.nbin", ModelPath + @"/Parser/tagdict");
            var tokens = Tokenizer.Tokenize(sentence);
            var taggedList = posTagger.Tag(tokens);
            IList<Tuple<string, string>> tagged = new List<Tuple<string, string>>();
            for (int i = 0; i < tokens.Length; i++)
            {
                tagged.Add(Tuple.Create(tokens[i], taggedList[i]));
            }
            return GetFilteredTokens(tagged);
        }

    }
}
