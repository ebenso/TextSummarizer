using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRank.Helpers
{
    internal class ExtractUtil
    {
        public static ExtractUtil instance { get;  } = new ExtractUtil();

        private ExtractUtil()
        {
        }

        public IList<string> JoinAdjacentWords(IList<string> wordList, IList<string> keywordsList)
        {
            if (wordList.Count == keywordsList.Count)
                return null;

            var modifiedPhrases = new HashSet<string>();

            var dealtWith = new HashSet<string>();

            string firstWord = null, secondWord = null;

            for (int i = 0, j = 1; j < wordList.Count; i++, j++)
            {
                firstWord = wordList[i]; secondWord = wordList[j];

                if (keywordsList.Contains(firstWord) && keywordsList.Contains(secondWord))
                {
                    modifiedPhrases.Add($"{firstWord}  {secondWord}");
                    dealtWith.AddMultipleElements<string>(firstWord, secondWord);
                }
                else
                {
                    if (keywordsList.Contains(firstWord) && !dealtWith.Contains(firstWord))
                        modifiedPhrases.Add(firstWord);

                    //Last Word condition
                    if (j == wordList.Count - 1 && keywordsList.Contains(secondWord) && !dealtWith.Contains(secondWord))
                        modifiedPhrases.Add(secondWord);
                }
            }

            return modifiedPhrases.ToList();
        }

        private static IList<string> GetNormalizedList(IList<string> words)
        {
            return words.Select(x => x.Replace(".", "")).ToList();
        }

        public IList<string> GetNormalizedUniqueWordList(IList<Tuple<string, string>> keywords)
        {
            return GetNormalizedList(keywords.Select(x => x.Item1).ToList()).Distinct().ToList();
        }

    }
}
