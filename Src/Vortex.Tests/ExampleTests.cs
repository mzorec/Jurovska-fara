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
        public void SuccessDavcnaTest()
        {
           var model =  Register(new User() { Davcna = "5"});
            Assert.AreEqual(5, model.Davcna);
        }

        [Test]
        public void WrongDavcnaFormat()
        {
            var model = Register(new User() { Davcna = "aa" });
         
        }

        [Test]
        public void test()
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

        public Database.Models.User Register(User user)
        {
            user.UserName = "neakj";

            Database.Models.User dbUserModel = new Database.Models.User();
            dbUserModel.Username = user.UserName;
            dbUserModel.Email = user.Email;
            int davcna;
            if (int.TryParse(user.Davcna, out davcna))
            {
                dbUserModel.Davcna = davcna;
            }

            return dbUserModel;
        }

    }
}
