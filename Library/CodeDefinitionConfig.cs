using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CodeMeasureToolkit.Library
{
    public class CodeDefinitionConfig
    {
        public IList<string> FilesMustMatch { get; set; }

        public IList<string> CodeLineMustContain { get; set; }

        public IList<string> IgnoreFilesThatMatch { get; set; }

        public IList<string> IgnoreFoldersThatMatch { get; set; }

        public IList<string> IgnoreIfLineContainsOnly { get; set; }

        public IList<string> IgnoreLinesThatStartWith { get; set; }

        public IList<string> IgnoreLinesThatContains { get; set; }

        public static CodeDefinitionConfig FromJsonFile(string file)
        {
            return JsonConvert.DeserializeObject<CodeDefinitionConfig>(File.ReadAllText(file));
        }
    }
}
