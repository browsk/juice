using System;
using System.Collections.Generic;
using Juice.Core.Domain;

namespace Juice.Core.Domain
{
    public class Sprint : DomainObject<int>
    {
        public virtual string Name { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual Project Project { get; set; }
        public virtual IList<Task> Tasks { get; set; }
    }
}
