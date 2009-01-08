using Juice.Core.Domain;

namespace Juice.Core.Domain
{
    public class Project : DomainObject<int>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
