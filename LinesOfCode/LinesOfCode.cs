using CodeQualityToolkit.Library;

namespace CodeQualityToolkit.LinesOfCodeNamespace
{
    /// <summary>
    /// Responsible for counting lines of code in a string and producing output.
    /// </summary>
    public static class LinesOfCode
    {
        public static event StringOutput LineProcessComplete;

        public static int CountLinesOfCodeInFile(string file)
        {
            return Count(IOHelper.Read(file));
        }

        public static int Count(string input)
        {
            int loc = 0;
            foreach (string line in input.Split('\n'))
            {
                if (DoCount(line))
                {
                    loc++;
                    if (LineProcessComplete != null) LineProcessComplete(line.Trim('\r', '\n'));
                }
            }
            return loc;
        }

        /// <summary>
        /// Returns true if line is code.
        /// </summary>
        public static bool DoCount(string codeLine)
        {
            string trimmedLine = codeLine.ToLower().Trim(' ', '\t', '\r', '\n');
            return trimmedLine.Length != 0
                   && LineIsCode(trimmedLine);
        }

        private static bool LineIsCode(string codeLine)
        {
            foreach (string notCode in Rules.IgnoreLinesThatStartWith)
            {
                if (codeLine.StartsWith(notCode))
                {
                    return false;
                }
            }

            foreach (string notCode in Rules.IgnoreIfLineContainsOnly)
            {
                if (codeLine == notCode)
                {
                    return false;
                }
            }

            foreach (string cannotContain in Rules.IgnoreLineThatContains)
            {
                if (codeLine.Contains(cannotContain))
                {
                    return false;
                }
            }

            foreach (string mustContain in Rules.CodeLineMustContain)
            {
                if (!codeLine.Contains(mustContain))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
