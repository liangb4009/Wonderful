using Microsoft.EntityFrameworkCore;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 待处理项目上下文
    /// </summary>
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            :base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(s => new { s.TaskId });
            });
            modelBuilder.Entity<ScaleParams>(entity =>
            {
                entity.HasKey(s => new { s.ScaleParamId });
            });
            modelBuilder.Entity<ScaleValue>(entity =>
            {
                entity.HasKey(s => new { s.ScaleValueId });
            });

            modelBuilder.Entity<TodoItem>()
                .HasOne(t => t.ScaleParams)
                .WithOne(p => p.todoItem)
                .HasForeignKey<ScaleParams>(p=>p.TaskId);

            modelBuilder.Entity<ScaleValue>()
                .HasOne(s => s.TodoItem)
                .WithMany(t => t.ScaleValues)
                .HasForeignKey(s => s.TaskId);
                
        }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<ScaleValue> ScaleValues { get; set; }
        public DbSet<ScaleParams> ScaleParams { get; set; }
    }
}
