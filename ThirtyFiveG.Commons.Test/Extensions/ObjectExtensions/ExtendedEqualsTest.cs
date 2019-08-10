using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.ObjectExtensions
{
    [TestClass]
    public class ExtendedEqualsTest
    {
        [TestMethod]
        public void Non_number_and_non_number_equals()
        {
            char x = '1';
            char y = '1';

            Assert.IsTrue(x.ExtendedEquals(y));
            Assert.IsTrue(y.ExtendedEquals(x));
        }

        [TestMethod]
        public void Non_number_and_non_number_not_equals()
        {
            char x = '1';
            char y = '2';

            Assert.IsFalse(x.ExtendedEquals(y));
            Assert.IsFalse(y.ExtendedEquals(x));
        }

        [TestMethod]
        public void Number_and_non_number_not_equals()
        {
            int i = 1;
            char x = '1';

            Assert.IsFalse(x.ExtendedEquals(i));
            Assert.IsFalse(i.ExtendedEquals(x));
        }

        [TestMethod]
        public void Number_and_number_equals()
        {
            int i = 1;
            long l = 1;

            Assert.IsTrue(l.ExtendedEquals(i));
            Assert.IsTrue(i.ExtendedEquals(l));
        }

        [TestMethod]
        public void Number_and_number_not_equals()
        {
            int i = 1;
            long l = 2;

            Assert.IsFalse(l.ExtendedEquals(i));
            Assert.IsFalse(i.ExtendedEquals(l));
        }
    }
}
