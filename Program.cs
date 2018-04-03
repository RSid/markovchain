using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markov
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText(@"C:\Users\alla.hoffman\Desktop\the inevitable hoard\temp\heresun.txt");
            var sentenceLength = 100;
            var corpus = BuildCorpus(text);
            var creative = true;
            Console.WriteLine("Corpus Nommed.");
            
            Random rnd = new Random();
            int wordStartIndex = rnd.Next(0, corpus.Keys.Count - 1);
            string firstWord = corpus.Keys.ToList()[wordStartIndex];
            
            var message = firstWord;
            while (message.Split(" ").Length < sentenceLength)
            {
                var firstWordArray = corpus[firstWord];
                var nextWord = creative == true
                    ? CreativeNextWord(rnd, firstWordArray, corpus, firstWord)
                    : ConservativeNextWord(firstWordArray, corpus, firstWord);
                message += " " + nextWord;
                firstWord = nextWord;
            }
            
            Console.WriteLine(message);
        }

        private static string ConservativeNextWord(IEnumerable<string> firstWordArray, IReadOnlyDictionary<string, List<string>> corpus, string firstWord)
        {
            var mostFrequentlyUsed = firstWordArray.GroupBy(word => word)
                .Select(wordGroup => new { Word = wordGroup.Key, Frequency = wordGroup.Count() })
                .OrderByDescending(word => word.Frequency);
            var prunedList = mostFrequentlyUsed.Count() > 1 ? mostFrequentlyUsed.Where(x => x.Frequency > 1) : mostFrequentlyUsed;
            return CreativeNextWord(new Random(), prunedList.ToList(), corpus, firstWord);
        }

        private static string CreativeNextWord(Random rnd, ICollection firstWordArray, IReadOnlyDictionary<string, List<string>> corpus, string firstWord)
        {
            var nextWordIndex = firstWordArray.Count > 1 ? rnd.Next(0, firstWordArray.Count - 1) : 0;
            var nextWord = corpus[firstWord][nextWordIndex];
            return nextWord;
        }

        private static Dictionary<string, List<string>> BuildCorpus(string text)
        {
            var wordArray = text.Split(" ").Where(x => x != string.Empty || x.ToCharArray().All(y => y == ' '))
                .ToArray();
            var corpus = new Dictionary<string, List<string>>();
            for (int i = 0; i < wordArray.Length; i++)
            {
                var word = wordArray[i];
                if (!corpus.ContainsKey(word))
                {
                    if (i + 1 < wordArray.Length)
                    {
                        corpus.Add(word, new List<string> {wordArray[i + 1]});
                    }
                }
                else
                {
                    if (i + 1 < wordArray.Length)
                    {
                        corpus[word].Add(wordArray[i + 1]);
                    }
                }
            }

            return corpus;
        }
    }
}