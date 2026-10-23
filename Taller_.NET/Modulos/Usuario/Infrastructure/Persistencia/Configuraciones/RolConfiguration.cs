using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller_.NET.Modulos.Usuario;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia.Configuraciones
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(rol => rol.Id);

            builder.Property(rol => rol.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(rol => rol.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(Rol.LongitudMaximaNombre)
                .IsRequired();

            builder.HasIndex(rol => rol.Nombre)
                .IsUnique()
                .HasDatabaseName("ux_roles_nombre");

            builder.HasData(
                new Rol(1, Rol.NombreAdministrador),
                new Rol(2, Rol.NombreMantenimiento)
            );
        }
    }
}
