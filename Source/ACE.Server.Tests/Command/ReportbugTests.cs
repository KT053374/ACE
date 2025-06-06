using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACE.Server.Command.Handlers;

namespace ACE.Server.Tests.Command
{
    [TestClass]
    public class ReportbugTests
    {
        [TestMethod]
        public void CategoriesRequiringObjectAreRecognized()
        {
            Assert.IsTrue(PlayerCommands.ReportbugCategoryRequiresObject("creature"));
            Assert.IsTrue(PlayerCommands.ReportbugCategoryRequiresObject("npc"));
            Assert.IsTrue(PlayerCommands.ReportbugCategoryRequiresObject("item"));
            Assert.IsFalse(PlayerCommands.ReportbugCategoryRequiresObject("quest"));
        }

        [TestMethod]
        public void NormalizeBugDescription_TrimsWhitespace()
        {
            const string description = "test bug description   ";
            var normalized = PlayerCommands.NormalizeBugDescription(description);
            Assert.AreEqual("test bug description", normalized);
        }
    }
}
