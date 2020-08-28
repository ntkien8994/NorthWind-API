﻿using System;
using Microsoft.EntityFrameworkCore;
using NorthWind.Entity;
using NorthWind.Library;

namespace NorthWind.DL
{
    public partial class NorthWindContext<T> : DbContext where T:EntityBase, new()
    {
        public NorthWindContext()
        {
        }

        public NorthWindContext(DbContextOptions<NorthWindContext<T>> options)
            : base(options)
        {
        }
        public virtual DbSet<T> ListBase { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Config.GetAppSetting(AppSettingKey.CONNECTION_STRING);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }
        }
    }
}
