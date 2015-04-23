using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodeQualityToolkit.Library
{
   public class Rule : List<string>
    {
        public static Rule FromFile(string filename)
        {
            var r = new Rule();
            r.AddRange(ParseFile(filename));
            return r;
        }

       /// <summary>
       /// Rules should be separated by '\n'.
       /// </summary>
       private static IEnumerable<string> ParseFile(string filename)
       {
           string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
           if (location == null)
           {
               throw new FileNotFoundException("Could not locate assembly: " + Assembly.GetExecutingAssembly().Location);
           }

           string newFileName = Path.Combine(location, filename);
           if (File.Exists(newFileName))
           {
               filename = newFileName;
           }
           else
           {
               throw new FileNotFoundException("Could not find file: " + filename + " in " + location);
           }

           StreamReader sr = new StreamReader(filename);
           string[] fileContent = sr.ReadToEnd().Split('\n');
           foreach (string s in fileContent)
           {
               if (s != string.Empty)
               {
                   yield return s.Trim('\r');
               }
           }
       }
    }

   public static class Rules
   {
       private static Rule _ignoreLinesThatStartWith = new Rule();
       private static Rule _ignoreFoldersThatMatch = new Rule();
       private static Rule _excludeWords = new Rule();
       private static Rule _ignoreIfLineContainsOnly = new Rule();
       private static Rule _filesMustMatch = new Rule();
       private static Rule _ignoreFilesThatMatch = new Rule();
       private static Rule _codeLineMustContain = new Rule();
       private static Rule _ignoreLineThatContains = new Rule();

       public static Rule IgnoreLinesThatStartWith
       {
           get
           {
               if (_ignoreLinesThatStartWith.Any())
               {
                   return _ignoreLinesThatStartWith;
               }

               _ignoreLinesThatStartWith = Rule.FromFile("ignoreLinesThatStartWith.config");
               return _ignoreLinesThatStartWith;
           }
       }

       public static Rule IgnoreIfLineContainsOnly
       {
           get
           {
               if (_ignoreIfLineContainsOnly.Any())
               {
                   return _ignoreIfLineContainsOnly;
               }

               _ignoreIfLineContainsOnly = Rule.FromFile("ignoreIfLineContainsOnly.config");
               return _ignoreIfLineContainsOnly;
           }
       }

       public static Rule IgnoreLineThatContains
       {
           get
           {
               if (_ignoreLineThatContains.Any())
               {
                   return _ignoreLineThatContains;
               }

               _ignoreLineThatContains = Rule.FromFile("ignoreLineThatContains.config");
               return _ignoreLineThatContains;
           }
       }

       public static Rule CodeLineMustContain
       {
           get
           {
               if (_codeLineMustContain.Any())
               {
                   return _codeLineMustContain;
               }

               _codeLineMustContain = Rule.FromFile("codeLineMustContain.config");
               return _codeLineMustContain;
           }
       }

       public static Rule IgnoreFilesThatMatch
       {
           get
           {
               if (_ignoreFilesThatMatch.Any())
               {
                   return _ignoreFilesThatMatch;
               }

               _ignoreFilesThatMatch = Rule.FromFile("ignoreFilesThatMatch.config");
               return _ignoreFilesThatMatch;
           }
       }

       public static Rule FilesMustMatch
       {
           get
           {
               if (_filesMustMatch.Any())
               {
                   return _filesMustMatch;
               }

               _filesMustMatch = Rule.FromFile("filesMustMatch.config");
               return _filesMustMatch;
           }
       }

       public static Rule IgnoreFoldersThatMatch
       {
           get
           {
               if (_ignoreFoldersThatMatch.Any())
               {
                   return _ignoreFoldersThatMatch;
               }

               _ignoreFoldersThatMatch = Rule.FromFile("ignoreFoldersThatMatch.config");
               return _ignoreFoldersThatMatch;
           }
       }

       public static Rule ExcludeWords
       {
           get
           {
               if (_excludeWords.Any())
               {
                   return _excludeWords;
               }

               _excludeWords = Rule.FromFile("excludeWords.config");
               return _excludeWords;
           }
       }
   }
}
