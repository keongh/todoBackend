using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoBackend.Models
{

    public class TodoContext : DbContext
    {
       public DbSet<Todo> todos { get; set; }

       public TodoContext(DbContextOptions<TodoContext> options) : base(options)
       {
           
       }
    }
    public class Todo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Task { get; set; }
        
        public bool Completed { get; set; } 
    }
}