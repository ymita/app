using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public interface IAppDbContext
    {
        DbSet<Post> Posts { get; set; }
        DbSet<Tag> Tags { get; set; }
    }
}