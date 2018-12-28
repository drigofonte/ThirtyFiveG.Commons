using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using ThirtyFiveG.Commons.Extensions;

namespace ThirtyFiveG.Commons.Test.Extensions.ExpressionExtensions
{
    [TestClass]
    public class NotTest
    {
        [TestMethod]
        public void Not()
        {
            int variable = 1;
            Expression<Func<int, bool>> expression = (i) => i == variable;
            Expression<Func<int, bool>> notExpression = expression.Not();

            Assert.IsTrue(expression.Compile()(variable));
            Assert.IsFalse(notExpression.Compile()(variable));
        }
    }
}
