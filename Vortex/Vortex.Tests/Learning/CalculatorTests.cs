using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vortex.Tests.Learning
{
    public class CalculatorTests
    {
        [Test]
        public void CalculateTest()
        {
            ICalculator calc = new Calculator();
            int result = calc.Calculate("3 + 5");
            Assert.AreEqual(8, result);
        }
    }
}
