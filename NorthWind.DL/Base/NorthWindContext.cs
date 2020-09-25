using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
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
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractDetail> ContractDetails { get; set; }
        [NotMapped]
        public virtual DbSet<ContractDetailView> ContractDetailsView { get; set; }
        public virtual DbSet<Product> Products { get; set; }
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
