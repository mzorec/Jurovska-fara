using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Vortex.Database.Models;

namespace Vortex.Database.Repository
{
    public interface IUserRepositoy
    {
        User GetUser(long id);

        void AddUser(User user);
    }
}
