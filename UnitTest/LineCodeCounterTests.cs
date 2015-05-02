using CodeQualityToolkit.Library;
using CodeQualityToolkit.LinesOfCodeNamespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class LineCounterTest
    {
        [TestMethod]
        [DeploymentItem("codeDefinition.json")]
        public void ExcludeLinesThatOnlyContainBrackets()
        {
            RulesReader.FromFile("codeDefinition.json");

            Assert.IsFalse(LinesOfCode.DoCount("	}"));
            Assert.IsFalse(LinesOfCode.DoCount("{"));
            Assert.IsFalse(LinesOfCode.DoCount("{}"));

            Assert.IsTrue(LinesOfCode.DoCount("{ i=0;"));
        }

        [TestMethod]
        [DeploymentItem("codeDefinition.json")]
        public void ExcludeLinesWithComments()
        {
            RulesReader.FromFile("codeDefinition.json");

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
        [DeploymentItem("codeDefinition.json")]
        public void CountOneLine()
        {
            RulesReader.FromFile("codeDefinition.json");
            Assert.IsTrue(LinesOfCode.DoCount("i=0;\n"));
        }
    }
}
