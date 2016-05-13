using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomSimpleMembership.Models
{
    public abstract class Entity
    {
        public virtual int Id
        {
            get;
            set;
        }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual bool IsTransient()
        {
            return this.Id == default(int);
        }
    }
}