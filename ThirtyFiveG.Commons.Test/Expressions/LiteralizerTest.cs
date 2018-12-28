using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Linq.Expressions;
using ThirtyFiveG.Commons.Extensions;
using ThirtyFiveG.Commons.Expressions;

namespace ThirtyFiveG.Commons.Test.Expressions
{
    [TestClass]
    public class LiteralizerTest
    {
        private int _variable = 1;

        public int Variable { get { return 1; } }

        [TestMethod]
        public void Visit_private_variable()
        {
            Expression<Func<int, bool>> expression = (i) => i == _variable;
            Expression<Func<int, bool>> visited = (Expression<Func<int, bool>>)new Literalizer().Visit(expression);

            Assert.IsTrue(visited.Compile()(_variable));
            Assert.IsFalse(visited.Compile()(2));
        }

        [TestMethod]
        public void Visit_local_variable()
        {
            int variable = 1;

            Expression<Func<int, bool>> expression = (i) => i == variable;
            Expression<Func<int, bool>> visited = (Expression<Func<int, bool>>)new Literalizer().Visit(expression);

            Assert.IsTrue(visited.Compile()(variable));
            Assert.IsFalse(visited.Compile()(2));
        }

        [TestMethod]
        public void Visit_public_property()
        {
            Expression<Func<int, bool>> expression = (i) => i == Variable;
            Expression<Func<int, bool>> visited = (Expression<Func<int, bool>>)new Literalizer().Visit(expression);

            Assert.IsTrue(visited.Compile()(Variable));
            Assert.IsFalse(visited.Compile()(2));
        }

        [TestMethod]
        public void Visit_array()
        {
            int[] ints = new int[] { 1 };
            Expression<Func<int, bool>> expression = (i) => ints.Contains(i);
            Expression<Func<int, bool>> visited = (Expression<Func<int, bool>>)new Literalizer().Visit(expression);

            Assert.IsTrue(visited.Compile()(1));
            Assert.IsFalse(visited.Compile()(2));
        }
    }
}
