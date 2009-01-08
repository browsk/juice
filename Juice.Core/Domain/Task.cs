using Juice.Core.Domain;

namespace Juice.Core.Domain
{
    public class Task : DomainObject<int>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Sprint Sprint { get; set; }
        public virtual Story Story { get; set; }
    }
}
