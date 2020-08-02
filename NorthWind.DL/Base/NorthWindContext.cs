using System;
using Microsoft.EntityFrameworkCore;
using NorthWind.Entity;

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
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=KIENNGUYEN\\NTKIEN;Database=NorthWind;User Id=sa;Password=dunghoi@1234");
            }
        }
    }
}
