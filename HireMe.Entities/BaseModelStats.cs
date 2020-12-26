using System;

namespace HireMe.Entities
{
    public abstract class BaseModelStats
    {
        public virtual int Id { get; set; }

        public DateTime DateTime { get; set; }

        public int EntityId { get; set; }
    }
}
