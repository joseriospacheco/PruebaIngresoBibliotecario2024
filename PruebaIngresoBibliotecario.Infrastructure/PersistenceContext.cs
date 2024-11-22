using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PruebaIngresoBibliotecario.Core.Entities;

namespace PruebaIngresoBibliotecario.Infrastructure
{
    public class PersistenceContext : DbContext
    {

        private readonly IConfiguration Config;

        public DbSet<Prestamo> Prestamos { get; set; }

        public PersistenceContext(DbContextOptions<PersistenceContext> options, IConfiguration config) : base(options)
        {
            Config = config;
        }

        public async Task CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

    }
}
