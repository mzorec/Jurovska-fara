using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vortex.Database.Repository;
using Vortex.Models;

namespace Vortex.Controllers
{
    public class UserController : ApiController
    {
        private readonly IRepositoryFactory repositoryFactory;

        public UserController(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        [HttpGet]
        public User Login()
        {
            repositoryFactory.OpenSession();
            return new User { UserName =  "test " };
        }
    }
}
