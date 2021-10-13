using System;
using System.Linq.Expressions;
using CSLabs.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CSLabs.Api.Util
{
    public static class ModelBuilderExtensions
    {
        public static void TimeStamps<T> (this ModelBuilder builder) where T : Trackable
        {
            builder.Entity<T>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("UTC_TIMESTAMP()");
            builder.Entity<T>()
                .Property(b => b.UpdatedAt)
                .HasDefaultValueSql("UTC_TIMESTAMP()");
        }

        public static void Unique<TEntity>(this ModelBuilder builder, Expression<Func<TEntity, object>> indexExpression) where TEntity: class
        {
            builder.Entity<TEntity>().HasIndex(indexExpression).IsUnique();
        }

    }
}