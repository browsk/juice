using Juice.Core.Domain;

namespace Juice.Core.Domain
{
    public class User : DomainObject<int>
    {
        public virtual string Username { get; set; }
        public virtual string ApplicationName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
    }
}