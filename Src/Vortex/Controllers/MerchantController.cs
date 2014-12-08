using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vortex.Database.Models;

namespace Vortex.Controllers
{
    public class MerchantController : ApiController
    {
        public  IList<User> GetAllUsers()
        {
            return new List<User>() { new User { Username = "Test"}, new User() { Username = "Test2" } };
        }

        public User GetUser(long id)
        {
            return new User {Username = id.ToString()};
        }
    }
}
