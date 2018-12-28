using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThirtyFiveG.Commons.Tel
{
    [TestClass]
    public class DialingCodeTest
    {
        [TestMethod]
        public void Matches_alpha_2_true()
        {
            DialingCode code = new DialingCode("00", "AB", "CDE");
            Assert.IsTrue(code.Matches("AB"));
        }

        [TestMethod]
        public void Matches_alpha_2_false()
        {
            DialingCode code = new DialingCode("00", "AB", "CDE");
            Assert.IsFalse(code.Matches("FG"));
        }

        [TestMethod]
        public void Matches_alpha_3_true()
        {
            DialingCode code = new DialingCode("00", "AB", "CDE");
            Assert.IsTrue(code.Matches("CDE"));
        }

        [TestMethod]
        public void Matches_alpha_3_false()
        {
            DialingCode code = new DialingCode("00", "AB", "CDE");
            Assert.IsFalse(code.Matches("FGH"));
        }
    }
}
