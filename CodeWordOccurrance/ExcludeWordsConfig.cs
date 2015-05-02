using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CodeWordOccurrance
{
    public class ExcludeWordsConfig
    {
        public IList<string> ExcludeWords { get; set; }

        public static ExcludeWordsConfig FromJsonFile(string file)
        {
            return JsonConvert.DeserializeObject<ExcludeWordsConfig>(File.ReadAllText(file));
        }
    }
}
