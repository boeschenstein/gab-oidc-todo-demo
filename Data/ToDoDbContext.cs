using Microsoft.EntityFrameworkCore;

namespace Bif4DotNetDemo
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
          : base(options)
        {
            
        }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}