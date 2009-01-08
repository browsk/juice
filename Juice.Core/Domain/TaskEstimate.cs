using System;
using Juice.Core.Domain;

namespace Juice.Core.Domain
{
    public class TaskEstimate : DomainObject<int>
    {
        public virtual Task Task { get; set; }
        public virtual double Estimate { get; set; }
        public virtual DateTime TimeStamp { get; set; }
    }
}
