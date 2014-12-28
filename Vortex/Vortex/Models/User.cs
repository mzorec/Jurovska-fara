using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vortex.Models
{
    public class User
    {
        [MinLength(3)]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
    }
}