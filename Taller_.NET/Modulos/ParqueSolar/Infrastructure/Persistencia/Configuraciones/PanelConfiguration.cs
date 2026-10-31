using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Configuraciones
{
    public class PanelConfiguration : IEntityTypeConfiguration<PanelDomain>
    {
        public void Configure(EntityTypeBuilder<PanelDomain> builder)
        {
            builder.ToTable("Paneles");

            builder.HasKey(panel => panel.Id);

            builder.Property(panel => panel.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(panel => panel.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(PanelDomain.LongitudMaximaNombre)
                .IsRequired();

            builder.Property(panel => panel.FechaCreacion)
                .HasColumnName("fecha_creacion")
                .IsRequired();

            builder.Property(panel => panel.Estado)
                .HasColumnName("estado")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(panel => panel.Potencia)
                .HasColumnName("potencia")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(panel => panel.VidaUtil)
                .HasColumnName("vida_util")
                .HasMaxLength(PanelDomain.LongitudMaximaVidaUtil)
                .IsRequired();

            builder.Property(panel => panel.NodoId)
                .HasColumnName("nodo_id")
                .IsRequired();

            builder.HasOne(panel => panel.Nodo)
                .WithMany()
                .HasForeignKey(panel => panel.NodoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(panel => new { panel.Nombre, panel.NodoId })
                .IsUnique()
                .HasDatabaseName("ux_paneles_nombre_nodo");
        }
    }
}
