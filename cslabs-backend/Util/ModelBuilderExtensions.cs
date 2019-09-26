using System;
using System.Linq.Expressions;
using CSLabsBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CSLabsBackend.Util
{
    public static class ModelBuilderExtensions
    {
        public static void TimeStamps<T> (this ModelBuilder builder) where T : ITrackable
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

        public static void SnakeCaseDatabase(this ModelBuilder builder)
        {
            foreach(var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

                // Replace column names            
                foreach(var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = property.Relational().ColumnName.ToSnakeCase();
                }

                foreach(var key in entity.GetKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }

                foreach(var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }

                foreach(var index in entity.GetIndexes())
                {
                    index.Relational().Name = index.Relational().Name.ToSnakeCase();
                }
            }
        }
        
    }
}