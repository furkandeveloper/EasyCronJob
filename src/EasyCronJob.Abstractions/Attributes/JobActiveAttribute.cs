using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCronJob.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class JobActiveAttribute : Attribute
    {
        public bool Active { get; set; }
        public JobActiveAttribute(bool active = true)
        {
            this.Active = active;
        }
    }
}
