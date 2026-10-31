using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia
{
    public interface IParqueDbContext
    {
        DbSet<Parque> Parques { get; set; }

        DbSet<NodoDomain> Nodos { get; set; }

        DbSet<InversorDomain> Inversores { get; set; }

        DbSet<PanelDomain> Paneles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
