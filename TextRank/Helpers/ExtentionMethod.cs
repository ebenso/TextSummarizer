using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
