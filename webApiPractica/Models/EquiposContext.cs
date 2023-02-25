using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace webApiPractica.Models
{
    public class EquiposContext : DbContext

    {
        public EquiposContext(DbContextOptions<EquiposContext>options) : base(options)
        {

        }

        public DbSet<equipos> equipos { get; set; }
    }
}
