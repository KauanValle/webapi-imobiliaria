using Microsoft.EntityFrameworkCore;

namespace Imobiliaria.Model.DB
{
    class DbImobiliaria : DbContext
    {
        public DbImobiliaria(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbSet<Condominio> Condominios { get; set; } = null;
        public DbSet<Moradores> Moradores { get; set; } = null!;
        public DbSet<Cobranca> Cobranca { get; set; } = null!;
    }
}