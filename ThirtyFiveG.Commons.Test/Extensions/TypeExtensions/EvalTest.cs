using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.TypeExtensions
{
    [TestClass]
    public class EvalTest
    {
        [TestMethod]
        public void GetType_root()
        {
            Type t = typeof(MockEntity).Eval(".");
            Assert.AreEqual(typeof(MockEntity), t);
        }

        [TestMethod]
        public void GetType_one_to_one_navigation_property()
        {
            Type t = typeof(MockEntity).Eval(".RelationalEntity");
            Assert.AreEqual(typeof(MockRelationalEntity), t);
        }

        [TestMethod]
        public void GetType_one_to_many_navigation_property()
        {
            Type t = typeof(MockEntity).Eval(".AssociativeEntities[Guid=abc-123]");
            Assert.AreEqual(typeof(MockAssociativeEntity), t);
        }

        [TestMethod]
        public void GetType_unknown_property()
        {
            Should.Throw<ArgumentNullException>(() => typeof(MockEntity).Eval(".UnknownProperty"));
        }

        public class MockEntity
        {
            public MockRelationalEntity RelationalEntity { get; set; }
            public IEnumerable<MockAssociativeEntity> AssociativeEntities { get; set; }
        }

        public class MockRelationalEntity { }
        public class MockAssociativeEntity { }
    }
}
