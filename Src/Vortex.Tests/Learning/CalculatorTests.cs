using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Vortex.Tests.Learning
{
    class CalculatorTests 
    {

        [Test]
        public void Zracune()
        {
           ICalculator calc = new Calculator();
            int result = calc.izracunaj(1, 2);
            Assert.AreEqual(3, result);
        }
       
    }
}
