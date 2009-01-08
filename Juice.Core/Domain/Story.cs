using System.Collections.Generic;
using Juice.Core.Domain;

namespace Juice.Core.Domain
{
    public class Story : DomainObject<int>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Project Project { get; set; }
        public virtual IList<Task> Tasks { get; set; }
    }
}
