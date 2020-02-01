using System;
using CSLabs.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CSLabs.Api.Util
{
    public class ContextUtil
    {
        public static void UpdateTimeStamps(ChangeTracker changeTracker)
        {
            // Credit to:
            // https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
            var entries = changeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is Trackable trackable)
                {
                    DateTime now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedAt = now;
                            break;

                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.UpdatedAt = now;
                            break;
                    }
                }
            }
        }
    }
}