using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace webApiPractica.Models
{
    public class EquiposContext : DbContext

    {
        public EquiposContext(DbContextOptions<EquiposContext>options) : base(options)
        {

        }
        //Nombre de las tablas que existen en la base de datos
        public DbSet<equiposs> equiposs { get; set; }
        public DbSet<marcas> marcas { get; set; }
        public DbSet<tipo_equipo> tipo_equipo { get; set; }
        public DbSet<estados_equipo> estados_equipos { get; set; }
        public DbSet<estados_reserva> estados_reserva { get; set; }
        public DbSet<carreras> carreras { get; set; }
        public DbSet<facultades> facultades { get; set; }
        public DbSet<reservas> reservas { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
    }
}
