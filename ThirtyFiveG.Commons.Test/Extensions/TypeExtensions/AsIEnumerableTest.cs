using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.TypeExtensions
{
    [TestClass]
    public class AsIEnumerableTest
    {
        [TestMethod]
        public void AsIEnumerable_collection()
        {
            Assert.AreEqual(typeof(IEnumerable<string>), typeof(ICollection<string>).AsIEnumerable(true));
        }
    }
}
