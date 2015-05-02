using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeMeasureToolkit.Library;

namespace CodeMeasureToolkit.LinesOfCodeNamespace
{
    public static class FilesOfCode
    {
        public static event StringOutput FileFound;

        public static void Find(string directory)
        {
            foreach (string file in FindAllCodeFiles(directory))
            {
                if (FileFound != null)
                {
                    FileFound(file);
                }
            }
        }

        private static IEnumerable<string> FindAllCodeFiles(string searchIn)
        {
            try
            {
                List<string> matchingFiles = new List<string>();
                foreach (string filePattern in Rules.FilesMustMatch)
                {
                    string[] matches = Directory.GetFiles(searchIn, filePattern, SearchOption.AllDirectories);
                    foreach (string file in matches)
                    {
                        if (!IgnoreFolder(file))
                        {
                            matchingFiles.Add(file);
                        }
                    }
                }

                List<string> filesToIgnore = new List<string>();
                foreach (string ignoreFilePattern in Rules.IgnoreFilesThatMatch)
                {
                    filesToIgnore.AddRange(Directory.GetFiles(searchIn, ignoreFilePattern, SearchOption.AllDirectories));
                }

                return matchingFiles.Except(filesToIgnore);
            }
            catch (Exception e)
            {
                throw new Exception("Could not find files with current file search pattern: " + e.Message);
            }
        }

        private static bool IgnoreFolder(string file)
        {
            string fullDirectory = Path.GetDirectoryName(file);
            string[] directories = fullDirectory.Split('\\');
            foreach (string d in directories)
            {
                if (Rules.IgnoreFoldersThatMatch.Contains(d))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
