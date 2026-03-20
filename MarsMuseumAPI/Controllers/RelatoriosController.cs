using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarsMuseumAPI.Data;
using MarsMuseumAPI.Models;
using System.Linq;

namespace MarsMuseumAPI.Controllers
{
    /// <summary>
    /// Controller para geração de relatórios do museu
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly MarsMuseumContext _context;

        public RelatoriosController(MarsMuseumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gera um relatório completo com estatísticas do museu
        /// </summary>
        /// <returns>Dados estatísticos completos do museu</returns>
        /// <response code="200">Retorna o relatório completo</response>
        [HttpGet("Completo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> GetRelatorioCompleto()
        {
            // Total de visitantes
            var totalVisitantes = await _context.Visitantes.CountAsync();
            
            // Total de pesquisas respondidas
            var totalPesquisas = await _context.Pesquisas.CountAsync();
            
            // Média de tempo por exposição
            var mediaTempoExposicao = await _context.VisitasExposicao
                .GroupBy(v => v.IdExposicao)
                .Select(g => new
                {
                    IdExposicao = g.Key,
                    MediaSegundos = g.Average(v => v.DuracaoSegundos)
                })
                .ToListAsync();
            
            // Exposições mais visitadas
            var exposicoesVisitadas = await _context.VisitasExposicao
                .GroupBy(v => v.IdExposicao)
                .Select(g => new
                {
                    IdExposicao = g.Key,
                    TotalVisitas = g.Count()
                })
                .OrderByDescending(e => e.TotalVisitas)
                .ToListAsync();
            
            // Palavras-chave mais frequentes
            var palavrasChave = await _context.PalavrasChave
                .OrderByDescending(p => p.Frequencia)
                .Take(10)
                .ToListAsync();
            
            // Satisfação geral
            var satisfacaoGeral = await _context.Pesquisas
                .GroupBy(p => p.ExperienciaGeral)
                .Select(g => new
                {
                    Nivel = g.Key,
                    Quantidade = g.Count()
                })
                .ToListAsync();
            
            return new
            {
                TotalVisitantes = totalVisitantes,
                TotalPesquisas = totalPesquisas,
                MediaTempoExposicao = mediaTempoExposicao,
                ExposicoesVisitadas = exposicoesVisitadas,
                PalavrasChave = palavrasChave,
                SatisfacaoGeral = satisfacaoGeral,
                DataGeracao = DateTime.Now
            };
        }
    }
}
