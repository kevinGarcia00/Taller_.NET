using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller_.NET.Modulos.Historico.Domain;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia.Configuraciones
{
    public class HistoricoMantenibilidadConfiguration : IEntityTypeConfiguration<HistoricoMantenibilidad>
    {
        public void Configure(EntityTypeBuilder<HistoricoMantenibilidad> builder)
        {
            builder.ToTable("HistoricosMantenibilidad");

            builder.HasKey(registro => registro.Id);

            builder.Property(registro => registro.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(registro => registro.Fecha)
                .HasColumnName("fecha")
                .IsRequired();

            builder.Property(registro => registro.Observaciones)
                .HasColumnName("observaciones")
                .HasMaxLength(HistoricoMantenibilidad.LongitudMaximaObservaciones)
                .IsRequired();

            builder.Property(registro => registro.EstadoInicial)
                .HasColumnName("estado_inicial")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(registro => registro.EstadoResultante)
                .HasColumnName("estado_resultante")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.OwnsOne(registro => registro.Mantenedor, mantenedor =>
            {
                mantenedor.Property(valor => valor.IdMantenedor)
                    .HasColumnName("id_mantenedor")
                    .IsRequired();
            });

            builder.OwnsOne(registro => registro.ContextoPanel, contexto =>
            {
                contexto.Property(valor => valor.PanelId)
                    .HasColumnName("panel_id")
                    .IsRequired();

                contexto.Property(valor => valor.NodoId)
                    .HasColumnName("nodo_id")
                    .IsRequired();

                contexto.Property(valor => valor.ParqueSolarId)
                    .HasColumnName("parque_solar_id")
                    .IsRequired();

                contexto.HasIndex(valor => valor.PanelId)
                    .HasDatabaseName("ix_historicos_mantenibilidad_panel_fecha");
            });

            builder.Navigation(registro => registro.Mantenedor).IsRequired();
            builder.Navigation(registro => registro.ContextoPanel).IsRequired();
        }
    }
}
