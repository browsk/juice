using System.Collections.Generic;

namespace Juice.Core.Domain
{
    public class Project : DomainObject<int>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Sprint> Sprints { get; set; }
    }
}
