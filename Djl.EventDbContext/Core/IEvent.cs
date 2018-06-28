using System;

namespace Djl.EventDbContext.Core
{
    public interface IEvent
    {
        Guid Id { get; set; }
        DateTime CreatiedDateTime { get; set; }
    }
}