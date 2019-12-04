using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oniqys.Proxy
{
    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public void AccessTest()
        {
            var intPropertyGenerator = DynamicPropertyFactory.Create<List<int>, int>();

            var countPropertyInfo = typeof(List<int>).GetProperty(nameof(List<int>.Count));

            var getter = intPropertyGenerator.CreateGetter(countPropertyInfo);

            var list = new List<int> { 1, 2, 3 };

            var result = getter(list);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void InterfaceTest()
        {
            var intPropertyGenerator = DynamicPropertyFactory.Create(typeof(List<int>), typeof(int));

            var countPropertyInfo = typeof(List<int>).GetProperty(nameof(List<int>.Count));

            var getter = (Func<List<int>, int>)intPropertyGenerator.CreateGetter(countPropertyInfo);

            var list = new List<int> { 1, 2, 3 };

            var result = getter(list);

            Assert.AreEqual(3, result);
        }
    }
}
