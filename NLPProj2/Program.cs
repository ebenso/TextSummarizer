using System;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.Tokenize;

namespace NLPProj2
{
    class Program
    {
        static void Main(string[] args)
        {
            var sentence = "- Sorry Mrs. Hudson, I'll skip the tea.";
            new ExtractKeyPhrases().Extract(sentence);
            
            Console.WriteLine("Hello World!");
        }
    }
}
