using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FastFood.Models;

namespace FastFood.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Food> Foods { get; set; }

    public DbSet<Order> Orders { get; set; }
    
    public DbSet<User> Users { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }
    
    public DbSet<ContactMessage> ContactMessages { get; set; }
    
    public DbSet<Contact> Contacts { get; set; }
}