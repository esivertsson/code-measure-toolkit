using CodeQualityToolkit.LinesOfCodeNamespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class LineCounterTest
    {
        [TestMethod]
        public void ExcludeLinesThatOnlyContainBrackets()
        {
            Assert.IsFalse(LinesOfCode.DoCount("	}"));
            Assert.IsFalse(LinesOfCode.DoCount("{"));
            Assert.IsFalse(LinesOfCode.DoCount("{}"));

            Assert.IsTrue(LinesOfCode.DoCount("{ i=0;"));
        }

        [TestMethod]
        public void ExcludeLinesWithComments()
        {
            Assert.IsFalse(LinesOfCode.DoCount("//"));
            Assert.IsFalse(LinesOfCode.DoCount("   //"));
            Assert.IsFalse(LinesOfCode.DoCount("///"));
        }

        [TestMethod]
        public void DoNotCountEmptyLines()
        {
            Assert.IsFalse(LinesOfCode.DoCount("\n"));
            Assert.IsFalse(LinesOfCode.DoCount(string.Empty));
        }

        [TestMethod]
        public void CountOneLine()
        {
            Assert.IsTrue(LinesOfCode.DoCount("i=0;\n"));
        }
    }
}
