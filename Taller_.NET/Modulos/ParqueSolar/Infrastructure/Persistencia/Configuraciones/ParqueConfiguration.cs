using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller_.NET.Modulos.ParqueSolar.Domain;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Configuraciones
{
    public class ParqueConfiguration : IEntityTypeConfiguration<Parque>
    {
        public void Configure(EntityTypeBuilder<Parque> builder)
        {
            builder.ToTable("Parques");

            builder.HasKey(parque => parque.Id);

            builder.Property(parque => parque.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(parque => parque.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(Parque.LongitudMaximaNombre)
                .IsRequired();

            builder.Property(parque => parque.Ubicacion)
                .HasColumnName("ubicacion")
                .HasMaxLength(Parque.LongitudMaximaUbicacion)
                .IsRequired();

            builder.Property(parque => parque.FechaCreacion)
                .HasColumnName("fecha_creacion")
                .IsRequired();

            builder.Property(parque => parque.CantMaxNodos)
                .HasColumnName("cant_max_nodos")
                .IsRequired();

            builder.HasIndex(parque => parque.Nombre)
                .IsUnique()
                .HasDatabaseName("ux_parques_nombre");
        }
    }
}
