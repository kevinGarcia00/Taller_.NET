using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Configuraciones
{
    public class InversorConfiguration : IEntityTypeConfiguration<InversorDomain>
    {
        public void Configure(EntityTypeBuilder<InversorDomain> builder)
        {
            builder.ToTable("Inversores");

            builder.HasKey(inversor => inversor.Id);

            builder.Property(inversor => inversor.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(inversor => inversor.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(InversorDomain.LongitudMaximaNombre)
                .IsRequired();

            builder.Property(inversor => inversor.FechaCreacion)
                .HasColumnName("fecha_creacion")
                .IsRequired();
        }
    }
}
