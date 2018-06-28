using System;
using Microsoft.EntityFrameworkCore;

namespace Djl.EventDbContext
{
    public class EventDbcontext : DbContext
    {
        public DbSet<MqEventMessage> MqEventMessages { get; set; }

        public EventDbcontext(DbContextOptions<EventDbcontext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MqEventMessageMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
