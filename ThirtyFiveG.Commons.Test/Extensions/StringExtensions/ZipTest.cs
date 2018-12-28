using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.StringExtensions
{
    [TestClass]
    public class ZipTest
    {
        [TestMethod]
        public void ZipUnzip()
        {
            string str = "a";
            byte[] zippedStr = str.Zip();
            string unzippedStr = zippedStr.Unzip();

            Assert.AreEqual(str, unzippedStr);
        }
    }
}
