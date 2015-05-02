using CodeQualityToolkit.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class RuleTests
    {
        [TestMethod]
        [DeploymentItem("codeDefinition.json")]
        public void ReadConfigFromFile()
        {
            CodeDefinitionConfig config = CodeDefinitionConfig.FromJsonFile("codeDefinition.json");

            Assert.IsNotNull(config);
            Assert.AreEqual(1, config.FilesMustMatch.Count);
            Assert.AreEqual(0, config.CodeLineMustContain.Count);
            Assert.AreEqual(3, config.IgnoreFilesThatMatch.Count);
            Assert.AreEqual(3, config.IgnoreFoldersThatMatch.Count);
            Assert.AreEqual(8, config.IgnoreIfLineContainsOnly.Count);
            Assert.AreEqual(6, config.IgnoreLinesThatStartWith.Count);
            Assert.AreEqual(2, config.IgnoreLinesThatContains.Count);
        }

        [TestMethod]
        [DeploymentItem("codeDefinition.json")]
        public void ReadConfigWithRulesReader()
        {
            RulesReader.FromFile("codeDefinition.json");

            Assert.AreEqual(1, Rules.FilesMustMatch.Count);
            Assert.AreEqual(0, Rules.CodeLineMustContain.Count);
            Assert.AreEqual(3, Rules.IgnoreFilesThatMatch.Count);
            Assert.AreEqual(3, Rules.IgnoreFoldersThatMatch.Count);
            Assert.AreEqual(8, Rules.IgnoreIfLineContainsOnly.Count);
            Assert.AreEqual(6, Rules.IgnoreLinesThatStartWith.Count);
            Assert.AreEqual(2, Rules.IgnoreLinesThatContains.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), "Config file 'doesNotExist.json' does not exist")]
        public void When_Json_File_Does_Not_Exist_Then_Exception_Is_Thrown()
        {
            RulesReader.FromFile("doesNotExist.json");
        }

        [TestMethod]
        [DeploymentItem("empty.json")]
        public void When_Json_File_Is_Empty_Then_Rules_Are_Empty()
        {
            RulesReader.FromFile("empty.json");
            
            Assert.AreEqual(0, Rules.FilesMustMatch.Count);
            Assert.AreEqual(0, Rules.CodeLineMustContain.Count);
            Assert.AreEqual(0, Rules.IgnoreFilesThatMatch.Count);
            Assert.AreEqual(0, Rules.IgnoreFoldersThatMatch.Count);
            Assert.AreEqual(0, Rules.IgnoreIfLineContainsOnly.Count);
            Assert.AreEqual(0, Rules.IgnoreLinesThatStartWith.Count);
            Assert.AreEqual(0, Rules.IgnoreLinesThatContains.Count);
        }
    }
}
