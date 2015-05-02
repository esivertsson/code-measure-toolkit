using System;
using System.Collections.Generic;

namespace CodeQualityToolkit.Library
{
    public static class Rules
    {
        public static IList<string> FilesMustMatch { get; set; }

        public static IList<string> CodeLineMustContain { get; set; }

        public static IList<string> IgnoreFilesThatMatch { get; set; }

        public static IList<string> IgnoreFoldersThatMatch { get; set; }

        public static IList<string> IgnoreIfLineContainsOnly { get; set; }

        public static IList<string> IgnoreLinesThatStartWith { get; set; }

        public static IList<string> IgnoreLinesThatContains { get; set; }
    }

    public class RulesReader
    {
        public static void FromFile(string file)
        {
            if (!IOHelper.IsThisAFile(file))
            {
                throw new Exception(string.Format("Config file '{0}' does not exist", file));
            }

            CodeDefinitionConfig config = CodeDefinitionConfig.FromJsonFile(file);
            Rules.FilesMustMatch = config.FilesMustMatch ?? new List<string>();
            Rules.CodeLineMustContain = config.CodeLineMustContain ?? new List<string>();
            Rules.IgnoreFilesThatMatch = config.IgnoreFilesThatMatch ?? new List<string>();
            Rules.IgnoreFoldersThatMatch = config.IgnoreFoldersThatMatch ?? new List<string>();
            Rules.IgnoreIfLineContainsOnly = config.IgnoreIfLineContainsOnly ?? new List<string>();
            Rules.IgnoreLinesThatContains = config.IgnoreLinesThatContains ?? new List<string>();
            Rules.IgnoreLinesThatStartWith = config.IgnoreLinesThatStartWith ?? new List<string>();
        }
    }
}
