using System;
using System.Collections.Generic;
using System.Text;

namespace Djl.EventDbContext.Core
{
    public abstract class IntegrationEvent : IEvent
    {
        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatiedDateTime = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime CreatiedDateTime { get; set; }
    }
}
