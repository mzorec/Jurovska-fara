using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vortex.Database.Models
{
    public class User : EntityBase<User>
    {
        public virtual string Username { get; set; }

        public virtual string Email { get; set; }

        public virtual int Davcna { get; set; }

        public string Status { get; set; }

    }
}