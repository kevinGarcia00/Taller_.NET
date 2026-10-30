using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Configuraciones
{
    public class NodoConfiguration : IEntityTypeConfiguration<NodoDomain>
    {
        public void Configure(EntityTypeBuilder<NodoDomain> builder)
        {
            builder.ToTable("Nodos");

            builder.HasKey(nodo => nodo.Id);

            builder.Property(nodo => nodo.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(nodo => nodo.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(NodoDomain.LongitudMaximaNombre)
                .IsRequired();

            builder.Property(nodo => nodo.UbicacionParque)
                .HasColumnName("ubicacion_parque")
                .HasMaxLength(NodoDomain.LongitudMaximaUbicacionParque)
                .IsRequired();

            builder.Property(nodo => nodo.FechaCreacion)
                .HasColumnName("fecha_creacion")
                .IsRequired();

            builder.Property(nodo => nodo.EnergiaGenerada)
                .HasColumnName("energia_generada")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(nodo => nodo.InversorId)
                .HasColumnName("inversor_id");

            builder.Property(nodo => nodo.ParqueSolarId)
                .HasColumnName("parque_solar_id")
                .IsRequired();

            builder.HasOne(nodo => nodo.Inversor)
                .WithOne()
                .HasForeignKey<NodoDomain>(nodo => nodo.InversorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Ignore(nodo => nodo.ContextoParque);

            builder.HasIndex(nodo => new { nodo.Nombre, nodo.ParqueSolarId })
                .IsUnique()
                .HasDatabaseName("ux_nodos_nombre_parque");

            builder.HasIndex(nodo => nodo.InversorId)
                .IsUnique()
                .HasFilter("inversor_id IS NOT NULL")
                .HasDatabaseName("ux_nodos_inversor_id");
        }
    }
}
