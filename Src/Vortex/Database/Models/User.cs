using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vortex.Database.Models
{
    public class User : EntityBase<User>
    {
        public virtual string Username { get; set; }
    }
}