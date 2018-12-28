using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ThirtyFiveG.Commons.Tel
{
    [TestClass]
    public class DialingCodeCollectionTest
    {
        [TestMethod]
        public void MatchDialingCode_match_found()
        {
            DialingCode dialingCode = new DialingCode("44", "GB", "GBR");
            DialingCode[] dialingCodes = new DialingCode[] { dialingCode };
            Mock<DialingCodeCollection> mockCollection = new Mock<DialingCodeCollection>();
            mockCollection.SetupGet(m => m.DialingCodes).Returns(dialingCodes);

            Assert.AreEqual(dialingCode, mockCollection.Object.MatchDialingCode("441141234567"));
        }

        [TestMethod]
        public void MatchDialingCode_match_not_found()
        {
            DialingCode[] dialingCodes = new DialingCode[] { new DialingCode("44", "GB", "GBR") };
            Mock<DialingCodeCollection> mockCollection = new Mock<DialingCodeCollection>();
            mockCollection.SetupGet(m => m.DialingCodes).Returns(dialingCodes);

            Assert.IsNull(mockCollection.Object.MatchDialingCode("551141234567"));
        }

        [TestMethod]
        public void MatchDialingCode_find_longest_match()
        {
            DialingCode ukDialingCode = new DialingCode("44", "GB", "GBR");
            DialingCode guernseyDialingCode = new DialingCode("44-1481", "GG", "GGY");
            DialingCode[] dialingCodes = new DialingCode[] { ukDialingCode, guernseyDialingCode };
            Mock<DialingCodeCollection> mockCollection = new Mock<DialingCodeCollection>();
            mockCollection.SetupGet(m => m.DialingCodes).Returns(dialingCodes);

            Assert.AreEqual(guernseyDialingCode, mockCollection.Object.MatchDialingCode("44-14811141234567"));
        }
    }
}
