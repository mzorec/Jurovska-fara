using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vortex.Models;

namespace Vortex.Controllers
{
    public class UserController : ApiController
    {
        public User Login()
        {
            return new User { UserName =  "test " };
        }
    }
}
