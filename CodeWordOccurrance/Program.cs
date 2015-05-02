using System;
using System.Collections.Generic;
using System.Linq;
using CodeQualityToolkit.Library;
using CodeQualityToolkit.LinesOfCodeNamespace;
using CodeWordOccurrance;

namespace CodeQualityToolkit.CodeWordOccurrance
{
    internal class Program
    {
        private const int THRESHOLD = 4;
        
        private static readonly Dictionary<string, int> _occurrances = new Dictionary<string, int>();

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            if (!IOHelper.IsThisADirectory(args[0]))
            {
                return;
            }

            string directory = args[0];

            RulesReader.FromFile("codeDefinition.json");
            ExcludeWordsConfig excludeWordsConfig = ExcludeWordsConfig.FromJsonFile("excludeWords.json");

            LinesOfCode.LineProcessComplete += LinesOfCode_LineProcessComplete;
            FilesOfCode.FileFound += (f) => LinesOfCode.CountLinesOfCodeInFile(f);
            FilesOfCode.Find(directory);

            foreach (KeyValuePair<string, int> k in _occurrances
                .Where(kvp => kvp.Value > THRESHOLD)
                .Where(kvp => !excludeWordsConfig.ExcludeWords.Contains(kvp.Key))
                .OrderByDescending(kvp => { return kvp.Value; }))
            {
                Console.WriteLine(k.Value + ":\t" + k.Key);
            }
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void LinesOfCode_LineProcessComplete(string line)
        {
            string[] words = line.Trim('\t', '\r', ')', '(', ':', ';').Split(' ');
            foreach (string word in words)
            {
                string trimmedWord = word.Trim(' ');
                if (trimmedWord == string.Empty)
                {
                    continue;
                }

                if (_occurrances.ContainsKey(trimmedWord))
                {
                    _occurrances[trimmedWord]++;
                }
                else
                {
                    _occurrances.Add(trimmedWord, 1);
                }
            }
        }
    }
}
