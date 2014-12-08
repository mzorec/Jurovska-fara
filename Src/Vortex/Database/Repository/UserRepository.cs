using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using Vortex.Database.Models;

namespace Vortex.Database.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepositoy
    {
        public UserRepository(ISession session, ITimeProvider timeProvider) : base(session, timeProvider)
        {
        }

        public User GetUser(long id)
        {
           return Get(id);
        }

        public void AddUser(User user)
        {
            Add(user);
        }
    }
}