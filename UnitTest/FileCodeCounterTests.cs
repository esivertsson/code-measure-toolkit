using System.IO;
using CodeMeasureToolkit.LinesOfCodeNamespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class FileCodeCounterTests
    {
        private string _testFile = "";

        [TestInitialize]
        public void RunBeforeAnyTest()
        {
            _testFile = Path.GetTempFileName();
            var handle = File.CreateText(_testFile);
            handle.Close();
        }

        [TestCleanup]
        public void RunAfterAnyTest()
        {
            File.Delete(_testFile);
        }

        [TestMethod]
        public void MethodWithAttribute_AttributeDoesNotCount()
        {
            AddLineToFile("[STAThread]");
            AddLineToFile("static void Main()");
            AddLineToFile("{");
            AddLineToFile("}");

            int nrOfCodeLines = LinesOfCode.CountLinesOfCodeInFile(_testFile);
            Assert.AreEqual(1, nrOfCodeLines);
        }

        [TestMethod]
        public void MethodWithDocComments_CommentsDoesNotCount()
        {
            AddLineToFile("    /// <summary>");
            AddLineToFile("/// The main entry point for the application.");
            AddLineToFile("/// </summary>");
            AddLineToFile("static void Main()");

            int nrOfCodeLines = LinesOfCode.CountLinesOfCodeInFile(_testFile);
            Assert.AreEqual(1, nrOfCodeLines);
        }

        private void AddLineToFile(string content)
        {
            using (TextWriter writer = File.AppendText(_testFile))
            {
                writer.WriteLine(content);
            }
        }
    }
}
