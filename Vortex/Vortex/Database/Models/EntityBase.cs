using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vortex.Database.Models
{
    public abstract class EntityBase<T>
    {
        protected EntityBase()
        {
        }

        protected EntityBase(long id)
        {
            this.Id = id;
        }

        public virtual long Id
        {
            get;
            protected set;
        }

        public virtual DateTime CreatedTime { get; set; }

        public virtual DateTime? ModifiedTime { get; set; }
    }
}