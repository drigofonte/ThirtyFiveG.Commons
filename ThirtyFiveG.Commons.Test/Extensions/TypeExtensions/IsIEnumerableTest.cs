using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.TypeExtensions
{
    [TestClass]
    public class IsIEnumerableTest
    {
        [TestMethod]
        public void IsIEnumerable_generic()
        {
            Assert.IsTrue(typeof(IEnumerable<object>).IsIEnumerable());
        }

        [TestMethod]
        public void IsIEnumerable_non_generic()
        {
            Assert.IsTrue(typeof(IEnumerable).IsIEnumerable());
        }

        [TestMethod]
        public void IsIEnumerable_enumerable_implementation()
        {
            Assert.IsTrue(typeof(ICollection<object>).IsIEnumerable());
        }

        [TestMethod]
        public void IsIEnumerable_non_enumerable_implementation()
        {
            Assert.IsFalse(typeof(object).IsIEnumerable());
        }
    }
}
