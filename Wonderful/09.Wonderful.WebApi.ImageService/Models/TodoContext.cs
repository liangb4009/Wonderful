using Microsoft.EntityFrameworkCore;
using System;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 待处理项目上下文
    /// </summary>
    public class TodoContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>(entity=> {
                entity.HasKey(s => new { s.TaskId });
            });
            modelBuilder.Entity<ImageValue>(entity=> {
                entity.HasKey(s => new { s.Id });
            });
            modelBuilder.Entity<ImageValue>()
                .HasOne(i => i.TodoItem)
                .WithMany(t => t.ImageValues)
                .HasForeignKey(i => i.TaskId);

            modelBuilder.Entity<TodoItem>().Property(m => m.CreateDate).HasDefaultValue(DateTime.Now);
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<TodoItem> TodoItems { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<ImageValue> ImageValues { get; set; }
    }
}
