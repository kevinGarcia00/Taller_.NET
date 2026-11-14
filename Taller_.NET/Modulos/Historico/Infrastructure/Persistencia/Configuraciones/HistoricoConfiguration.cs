using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EntidadHistorico = Taller_.NET.Modulos.Historico.Domain.Historico;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia.Configuraciones
{
    public class HistoricoConfiguration : IEntityTypeConfiguration<EntidadHistorico>
    {
        public void Configure(EntityTypeBuilder<EntidadHistorico> builder)
        {
            builder.ToTable("Registros");

            builder.HasKey(registro => registro.Id);

            builder.Property(registro => registro.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(registro => registro.Fecha)
                .HasColumnName("fecha")
                .IsRequired();

            builder.Property(registro => registro.EnergiaTotal)
                .HasColumnName("energia_total")
                .HasPrecision(18, 2)
                .IsRequired();

            // Referencia al panel sin navegar a las entidades de ParqueSolar.
            builder.Property(registro => registro.PanelId)
                .HasColumnName("panel_id")
                .IsRequired();

            builder.Property(registro => registro.NodoId)
                .HasColumnName("nodo_id")
                .IsRequired();

            builder.Property(registro => registro.ParqueSolarId)
                .HasColumnName("parque_solar_id")
                .IsRequired();

            builder.Ignore(registro => registro.ContextoPanel);

            builder.HasIndex(registro => new { registro.PanelId, registro.Fecha })
                .HasDatabaseName("ix_registros_panel_fecha");
        }
    }
}
