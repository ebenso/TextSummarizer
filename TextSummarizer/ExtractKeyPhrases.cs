using System;
using System.Collections.Generic;
using System.Linq;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Tokenize;

namespace NLPProj2
{
    class ExtractKeyPhrases
    {
        private readonly string _modelPath;
        private readonly EnglishRuleBasedTokenizer _tokenizer;
        private readonly EnglishMaximumEntropySentenceDetector _sentence_tokenizer;

        private static readonly IList<string> _requiredTags = new List<string> { "NN", "JJ", "NNP", "NNS", "NNPS" };

        public ExtractKeyPhrases()
        {
            _modelPath = AppDomain.CurrentDomain.BaseDirectory + "../../Resources/Models/";
            _tokenizer = new EnglishRuleBasedTokenizer(false);
            _sentence_tokenizer = new EnglishMaximumEntropySentenceDetector(_modelPath + "/EnglishSD.nbin");
        }

        private IList<Tuple<string, string>> GetFilteredTokens(IList<Tuple<string, string>> taggedTokens)
        {
            for (int i = taggedTokens.Count-1; i >= 0;i--)
            {
                if (_requiredTags.All(x => x != taggedTokens[i].Item2))
                {
                    taggedTokens.RemoveAt(i);
                }
            }
            return taggedTokens;
        }
        private IList<Tuple<string, string>> GetPosTaggedTokens(string sentence)
        {
            var posTagger =  new EnglishMaximumEntropyPosTagger(_modelPath + "/EnglishPOS.nbin", _modelPath + @"/Parser/tagdict");
            var tokens = _tokenizer.Tokenize(sentence);
            var taggedList = posTagger.Tag(tokens);
            IList<Tuple<string, string>> tagged = new List<Tuple<string, string>>();
            for (int i = 0; i < tokens.Length; i++)
            {
                tagged.Add(Tuple.Create(tokens[i], taggedList[i]));
            }
            return GetFilteredTokens(tagged);
        }
        //ref: stackoverflow.com/questions/1952153/
        private static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new [] { t2 }));
        }

        private IList<string> GetNormalizedList(IList<string> words)
        {
            return words.Select(x => x.Replace(".", "")).ToList();
        }

        private IList<Tuple<string,string>> BuildGraph(IList<string> words)
        {
            return  GetKCombs(words, 2).Select(x =>
            {
                var enumerable = x as string[] ?? x.ToArray();
                return new Tuple<string, string>(enumerable.ToList()[0], enumerable.ToList()[1]);
            }).ToList(); ;
        }

        private IList<string> GetNormalizedUniqueWordList(IList<Tuple<string, string>> keywords)
        {
            var temp = GetNormalizedList(keywords.Select(x => x.Item1).ToList());
            return temp.Distinct().ToList();
        }
       
        public void Extract(string sentence)
        {
            var taggedList = GetPosTaggedTokens(sentence);
            var uniqueWords = BuildGraph(GetNormalizedUniqueWordList(taggedList));
            var levDistance= new LevenhteinDistance();
            IList<Tuple<string, string, int>> graph = uniqueWords.Select(x =>
                new Tuple<string, string, int>(x.Item1, x.Item2, levDistance.Calculate(x.Item1, x.Item2))).ToList();

            var pqr = _sentence_tokenizer.SentenceDetect(
                @"To test easily the various NLP tools, run the ToolsExample winform project. You'll find below a more detailed description of the tools and how code snippets to use them directly in your code. All NLP tools based on the maxent algorithm need model files to run. You'll find those files for English in Resources/Models. If you want to train your own models (to improve precision on English or to use those tools on other languages), please refer to the last section.".Trim());
        }
    }
}
