using System;
using Microsoft.EntityFrameworkCore;

namespace Djl.EventDbContext
{
    public class MqEventDbcontext : DbContext
    {
        public DbSet<MqEventMessage> MqEventMessages { get; set; }

        public MqEventDbcontext(DbContextOptions<MqEventDbcontext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MqEventMessageMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
