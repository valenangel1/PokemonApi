namespace entregabletres.src.Config;

using Microsoft.EntityFrameworkCore;
using entregabletres.src.Entity;

class DbMemory : DbContext
{
    
    public DbMemory(DbContextOptions<DbMemory> options) : base(options) {}

    public DbSet<Pokemon> Pokemon => Set<Pokemon>();
}