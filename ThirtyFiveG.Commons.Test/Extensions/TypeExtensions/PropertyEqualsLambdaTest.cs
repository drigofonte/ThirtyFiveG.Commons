using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.TypeExtensions
{
    [TestClass]
    public class PropertyEqualsLambdaTest
    {
        [TestMethod]
        public void Non_nullable_property_equals()
        {
            // c => c.NonNullable == 1
            Expression<Func<MockClass, bool>> expression = typeof(MockClass).PropertyEqualsLambda<MockClass>("NonNullable", 1L);

            Assert.IsTrue(expression.Compile()(new MockClass() { NonNullable = 1 }));
        }

        [TestMethod]
        public void Non_nullable_property_does_not_equal()
        {
            // c => c.NonNullable == 1
            Expression<Func<MockClass, bool>> expression = typeof(MockClass).PropertyEqualsLambda<MockClass>("NonNullable", 1L);

            Assert.IsFalse(expression.Compile()(new MockClass() { NonNullable = 2 }));
        }

        [TestMethod]
        public void Nullable_property_equals()
        {
            // c => c.Nullable.HasValue == true && c.Nullable.Value == 1
            Expression<Func<MockClass, bool>> expression = typeof(MockClass).PropertyEqualsLambda<MockClass>("Nullable", 1L);

            Assert.IsTrue(expression.Compile()(new MockClass() { Nullable = 1 }));
        }

        [TestMethod]
        public void Nullable_property_does_not_have_value()
        {
            // c => c.Nullable.HasValue == true && c.Nullable.Value == 1
            Expression<Func<MockClass, bool>> expression = typeof(MockClass).PropertyEqualsLambda<MockClass>("Nullable", 1L);

            Assert.IsFalse(expression.Compile()(new MockClass() { Nullable = null }));
        }

        [TestMethod]
        public void Nullable_property_has_value_does_not_equal()
        {
            // c => c.Nullable.HasValue == true && c.Nullable.Value == 1
            Expression<Func<MockClass, bool>> expression = typeof(MockClass).PropertyEqualsLambda<MockClass>("Nullable", 1L);

            Assert.IsFalse(expression.Compile()(new MockClass() { Nullable = 2 }));
        }

        public class MockClass
        {
            public int NonNullable { get; set; }
            public int? Nullable { get; set; }
        }
    }
}
