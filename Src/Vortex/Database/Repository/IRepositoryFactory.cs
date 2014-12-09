using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Vortex.Database.Repository
{
    public interface IRepositoryFactory : IDisposable
    {
        ISession CurrentSession { get; }

        ISession OpenSession();

        IUserRepository CreateUserRepository();
    }
}
