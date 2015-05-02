using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CodeMeasureToolkit.Library;

namespace CodeMeasureToolkit.LinesOfCodeNamespace
{
    internal class Program
    {
        private static bool _verboseOutput;
        private static bool _minimalOutput;
        private static bool _orderedOutput;
        private static int _totalLinesOfCode;

        private static int Main(string[] args)
        {
            // Show change since last run (which files have a difference in loc?)

            // Break out ConsoleApplicationLogic and test it.
            // Solve the output-problem/Break out printing from Program
            // Refactor how parameters effect the output. Break out most of the logic.

            // Break out arguments analysis from Program.
            string path;
            if (args.Length > 0)
            {
                path = args[0];
            }
            else
            {
                PrintUsage();
                return 0;
            }

            RulesReader.FromFile("codeDefinition.json");

            if (args.Length > 1)
            {
                string lastArgument = args[args.Length - 1];
                _verboseOutput = lastArgument == "-v";
                _minimalOutput = lastArgument == "-m";
                _orderedOutput = lastArgument == "-o";
            }

            if (_verboseOutput)
            {
                LinesOfCode.LineProcessComplete += LineProcessComplete;
            }

            if (IOHelper.IsThisAFile(path))
            {
                int loc = ProcessFile(path);
                if (_minimalOutput)
                {
                    Console.WriteLine(loc);
                }
                return 0;
            }

            if (IOHelper.IsThisADirectory(path))
            {
                // TODO: Refactor!
                Dictionary<string, int> files = new Dictionary<string, int>();

                if (!_minimalOutput)
                {
                    Console.WriteLine();
                }

                if (_orderedOutput)
                {
                    FilesOfCode.FileFound += f => files.Add(f, CountFile(f));
                }
                else
                {
                    FilesOfCode.FileFound += f => ProcessFile(f);
                }

                FilesOfCode.Find(path);

                if (_orderedOutput)
                {
                    var orderedFiles = files.OrderByDescending(kvp => { return kvp.Value; });
                    foreach (KeyValuePair<string, int> k in orderedFiles)
                    {
                        FileProcessComplete(k.Key, k.Value);
                    }

                    Console.Write("\nTotal lines of code: ");
                }
                else if (!_minimalOutput)
                {
                    foreach (KeyValuePair<string, int> k in files)
                    {
                        FileProcessComplete(k.Key, k.Value);
                    }

                    Console.Write("\nTotal lines of code: ");
                }
                Console.WriteLine(_totalLinesOfCode);

#if DEBUG
                Console.ReadLine();
#endif
                return 0;
            }

            PrintUsage();
            return 1;
        }

        private static void LineProcessComplete(string line)
        {
            Console.WriteLine(line);
        }

        private static void FileProcessStarted(string file)
        {
            Console.WriteLine(file + ":");
        }

        private static void FileProcessComplete(string file, int nrOfLines)
        {
            Console.Write(file + ": ");
            int padLength = 70 - file.Length;
            if (padLength > 1)
            {
                for (int i = 0; i < padLength; i++)
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine(nrOfLines);
        }

        private static int ProcessFile(string file)
        {
            if (_verboseOutput)
            {
                FileProcessStarted(file);
            }

            int loc = CountFile(file);

            if (!_minimalOutput)
            {
                FileProcessComplete(file, loc);
            }
            return loc;
        }

        private static int CountFile(string file)
        {
            int loc = LinesOfCode.CountLinesOfCodeInFile(file);
            _totalLinesOfCode += loc;
            return loc;
        }

        private static void PrintUsage()
        {
            PresentApplication();
            Console.WriteLine("Usage: loc pattern [-v] [-m] [-o]");
            Console.WriteLine("\t pattern: Files to count. Can be a file or a specification to a directory.");
            Console.WriteLine("\t -v: Verbose output, all lines that are considered code are printed.");
            Console.WriteLine("\t -m: Minimal output, only the number of lines found are printed.");
            Console.WriteLine("\t -o: Ordered output, where files are ordered after number of lines");
            Console.WriteLine();
            Console.WriteLine("Example: 'loc .' counts all lines of code in current directory and all subdirectories and outputs the number of lines found for each file and the total number as well.");
            Console.WriteLine("Example: 'loc Program.cs -v' counts lines of code in Program.cs and outputs all lines that are considered code.");
        }

        private static void PresentApplication()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;
            Console.WriteLine("Lines of Code v." + version);
            Console.WriteLine();
        }
    }
}
