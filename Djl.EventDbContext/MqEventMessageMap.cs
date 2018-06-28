using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Djl.EventDbContext
{
    public class MqEventMessageMap : IEntityTypeConfiguration<MqEventMessage>
    {
        public void Configure(EntityTypeBuilder<MqEventMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatiedDateTime).IsRequired();
            builder.Property(x => x.AssemblyName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ClassFullName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Body).IsRequired().HasMaxLength(4000);
            builder.Property(x => x.PublishErrorMsg).HasMaxLength(1000);
        }
    }
}