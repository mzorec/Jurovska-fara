using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Vortex.Tests.Learning
{

    public interface ICalculator
    {
        int izracunaj(int a, int b);
    }

    public class Calculator : ICalculator
    {
        public int izracunaj(int a, int b)
        {
            int rezultat = a + b;
            return rezultat;
        }
    }
}
