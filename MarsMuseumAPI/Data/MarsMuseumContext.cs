using Microsoft.EntityFrameworkCore;
using MarsMuseumAPI.Models;

namespace MarsMuseumAPI.Data
{
    public class MarsMuseumContext : DbContext
    {
        public MarsMuseumContext(DbContextOptions<MarsMuseumContext> options)
            : base(options)
        {
        }

        public DbSet<Visitante> Visitantes { get; set; }
        public DbSet<Exposicao> Exposicoes { get; set; }
        public DbSet<VisitaExposicao> VisitasExposicao { get; set; }
        public DbSet<Pesquisa> Pesquisas { get; set; }
        public DbSet<PalavraChave> PalavrasChave { get; set; }
        public DbSet<LogSistema> LogsSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relacionamentos
            modelBuilder.Entity<VisitaExposicao>()
                .HasOne(ve => ve.Visitante)
                .WithMany(v => v.VisitasExposicao)
                .HasForeignKey(ve => ve.IdVisitante);

            modelBuilder.Entity<VisitaExposicao>()
                .HasOne(ve => ve.Exposicao)
                .WithMany(e => e.VisitasExposicao)
                .HasForeignKey(ve => ve.IdExposicao);

            modelBuilder.Entity<Pesquisa>()
                .HasOne(p => p.Visitante)
                .WithOne(v => v.Pesquisa)
                .HasForeignKey<Pesquisa>(p => p.IdVisitante);

            // Dados iniciais para exposições
            modelBuilder.Entity<Exposicao>().HasData(
                new Exposicao { Id = 1, Titulo = "Preparação para Marte", Descricao = "Como os astronautas se preparam para a missão", OrdemExibicao = 1 },
                new Exposicao { Id = 2, Titulo = "Viagem Interplanetária", Descricao = "A jornada de 7 meses até o planeta vermelho", OrdemExibicao = 2 },
                new Exposicao { Id = 3, Titulo = "Chegada em Marte", Descricao = "O pouso e os primeiros momentos em solo marciano", OrdemExibicao = 3 },
                new Exposicao { Id = 4, Titulo = "Exploração da Superfície", Descricao = "Descobertas e desafios na superfície de Marte", OrdemExibicao = 4 },
                new Exposicao { Id = 5, Titulo = "Habitação em Marte", Descricao = "Como será viver em uma colônia marciana", OrdemExibicao = 5 }
            );
        }
    }
}
