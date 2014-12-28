using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vortex.Controllers;
using Vortex.Database.Models;
using Vortex.Database.Repository;
using User = Vortex.Models.User;

namespace Vortex.Tests
{
    public class ExampleTests
    {
        [Test]
        public void Test()
        {
            List<int> seznam = new List<int>();
            seznam.Add(5);
            seznam.Add(4);
          
            int variabla = 0;
            for (int i = 0; i < seznam.Count; i++)
            {
                variabla = variabla + seznam[i];
            }
        }
    }
}
