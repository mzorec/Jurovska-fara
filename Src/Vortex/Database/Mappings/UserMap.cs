using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using Vortex.Database.Models;

namespace Vortex.Database.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(m => m.Id).GeneratedBy.HiLo("NH_Hilo", "Users_Next_Hi", "200");
            Map(m => m.Username).Not.Nullable().Length(80);
        }
    }
}