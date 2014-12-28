using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vortex.Services
{
    public interface IAuthenticationService
    {
        string CreateHash(string password, out string salt);

        bool ValidatePassword(string password, string salt, string correctHash);
    }
}
