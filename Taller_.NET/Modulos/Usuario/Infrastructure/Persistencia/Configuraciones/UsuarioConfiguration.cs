using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller_.NET.Modulos.Usuario;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia.Configuraciones
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(usuario => usuario.Id);

            builder.Property(usuario => usuario.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(usuario => usuario.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(Usuario.LongitudMaximaNombre)
                .IsRequired();

            builder.Property(usuario => usuario.Apellido)
                .HasColumnName("apellido")
                .HasMaxLength(Usuario.LongitudMaximaApellido)
                .IsRequired();

            builder.Property(usuario => usuario.Correo)
                .HasColumnName("correo")
                .HasMaxLength(Usuario.LongitudMaximaCorreo)
                .IsRequired();

            builder.Property(usuario => usuario.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(Usuario.LongitudMaximaTelefono)
                .IsRequired();

            builder.Property(usuario => usuario.Auth0UserId)
                .HasColumnName("auth0_user_id")
                .HasMaxLength(Usuario.LongitudMaximaAuth0UserId);

            builder.HasIndex(usuario => usuario.Correo)
                .IsUnique()
                .HasDatabaseName("ux_usuarios_correo");

            builder.HasIndex(usuario => usuario.Auth0UserId)
                .IsUnique()
                .HasFilter("auth0_user_id IS NOT NULL")
                .HasDatabaseName("ux_usuarios_auth0_user_id");

            builder.HasMany(usuario => usuario.Roles)
                .WithMany()
                .UsingEntity(usuarioRoles => usuarioRoles.ToTable("UsuarioRoles"));
        }
    }
}
