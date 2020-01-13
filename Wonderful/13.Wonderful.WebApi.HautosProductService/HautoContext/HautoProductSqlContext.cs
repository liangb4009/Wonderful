using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wonderful.WebApi.HautosProductService.HautosProduct.Entity;

namespace Wonderful.WebApi.HautosProductService.HautoContext
{
    public class HautoProductSqlContext : DbContext
    {
        public HautoProductSqlContext(DbContextOptions<HautoProductSqlContext> conn)
          : base(conn)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MSTB_COM_HUMITURE_HYGROTHERMOGRAPH>(entity => {
                entity.ToTable("MSTB_COM_HUMITURE_HYGROTHERMOGRAPH");
                entity.HasKey(p => new { p.EQUIPMENT_ID });
                entity.HasMany(p => p.TSTB_QUA_HUMITURE_DATAS).WithOne(c => c.MSTBCOMHUMITUREHYGROTHERMOGRAPH).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<TSTB_QUA_HUMITURE_DATA>(entity => {
                entity.ToTable("TSTB_QUA_HUMITURE_DATA");
                entity.HasKey(p => new { p.ID });
                entity.HasOne(c => c.MSTBCOMHUMITUREHYGROTHERMOGRAPH).WithMany(p => p.TSTB_QUA_HUMITURE_DATAS).OnDelete(DeleteBehavior.Cascade);


            });
        }
        public DbSet<MSTB_COM_HUMITURE_HYGROTHERMOGRAPH> MSTB_COM_HUMITURE_HYGROTHERMOGRAPHS { get; set; }
        public DbSet<TSTB_QUA_HUMITURE_DATA> TSTB_QUA_HUMITURE_DATAS { get; set; }


    }
}
