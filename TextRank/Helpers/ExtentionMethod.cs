using System.Collections.Generic;

namespace TextRank.Helpers
{
    public static class ExtentionMethod
    {
        public static HashSet<T> AddMultipleElements<T>(this HashSet<T> set, T firstWord, T secondWord)
        {
            set.Add(firstWord);
            set.Add(secondWord);
            return set;
        }
    }
}
