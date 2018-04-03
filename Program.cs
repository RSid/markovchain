using System;
using System.Collections.Generic;
using System.Linq;

namespace Markov
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText(@"C:\Users\alla.hoffman\Desktop\the inevitable hoard\temp\chatlog.txt");
            var sentenceLength = 15;
            var wordArray = text.Split(" ").Where(x=> x != string.Empty && x != " ").ToArray();
            var corpus = new Dictionary<string, List<string>>();
            for (int i=0;i<wordArray.Length;i++)
            {
                var word = wordArray[i];
                if (!corpus.ContainsKey(word))
                {
                    if (i + 1 < wordArray.Length)
                    {
                        corpus.Add(word, new List<string>{wordArray[i+1]});
                    }
                }
                else
                {
                    if (i + 1 < wordArray.Length)
                    {
                        corpus[word].Add(wordArray[i+1]);
                    }
                }
            }
            Console.WriteLine("Corpus Nommed.");
            
            Random rnd = new Random();
            int wordStartIndex = rnd.Next(0, corpus.Keys.Count - 1);
            string firstWord = corpus.Keys.ToList()[wordStartIndex];
            
            var message = firstWord;
            while (message.Split(" ").Length < sentenceLength)
            {
                var firsWordArray = corpus[firstWord];
                var nextWordIndex = rnd.Next(0, firsWordArray.Count - 1);
                var nextWord = corpus[firstWord][nextWordIndex];
                message += " " + nextWord;
                firstWord = nextWord;
            }
            
            Console.WriteLine(message);
        }
    }
}