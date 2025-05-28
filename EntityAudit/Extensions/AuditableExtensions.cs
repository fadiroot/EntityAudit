using System;
using EntityAudit.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityAudit.Extensions
{
    public static class AuditableExtensions
    {
        public static void ApplyAuditableConfiguration(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTime>(nameof(IAuditable.CreatedAt))
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property<string>(nameof(IAuditable.CreatedBy))
                        .HasMaxLength(256)
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTime?>(nameof(IAuditable.UpdatedAt));

                    modelBuilder.Entity(entityType.ClrType)
                        .Property<string>(nameof(IAuditable.UpdatedBy))
                        .HasMaxLength(256);
                }
            }
        }
    }
}
