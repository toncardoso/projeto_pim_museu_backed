using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarsMuseumAPI.Data;
using MarsMuseumAPI.Models;

namespace MarsMuseumAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de estatísticas do museu
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EstatisticasController : ControllerBase
    {
        private readonly MarsMuseumContext _context;

        public EstatisticasController(MarsMuseumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém estatísticas gerais para o painel administrativo
        /// </summary>
        /// <returns>Estatísticas resumidas do museu</returns>
        /// <response code="200">Retorna as estatísticas do museu</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> GetEstatisticas()
        {
            // Total de visitantes
            var totalVisitantes = await _context.Visitantes.CountAsync();
            
            // Total de visitantes hoje
            var hoje = DateTime.Today;
            var visitantesHoje = await _context.Visitantes
                .Where(v => v.DataHoraRegistro.Date == hoje)
                .CountAsync();
            
            // Total de pesquisas respondidas
            var totalPesquisas = await _context.Pesquisas.CountAsync();
            
            // Taxa de resposta de pesquisas
            double taxaResposta = totalVisitantes > 0 
                ? (double)totalPesquisas / totalVisitantes * 100 
                : 0;
            
            // Exposição mais visitada
            var exposicaoMaisVisitada = await _context.VisitasExposicao
                .GroupBy(v => v.IdExposicao)
                .Select(g => new
                {
                    IdExposicao = g.Key,
                    TotalVisitas = g.Count()
                })
                .OrderByDescending(e => e.TotalVisitas)
                .FirstOrDefaultAsync();
            
            string nomeExposicaoMaisVisitada = "Nenhuma";
            if (exposicaoMaisVisitada != null)
            {
                var exposicao = await _context.Exposicoes
                    .FindAsync(exposicaoMaisVisitada.IdExposicao);
                if (exposicao != null)
                {
                    nomeExposicaoMaisVisitada = exposicao.Titulo;
                }
            }
            
            return new
            {
                TotalVisitantes = totalVisitantes,
                VisitantesHoje = visitantesHoje,
                TotalPesquisas = totalPesquisas,
                TaxaRespostaPesquisa = Math.Round(taxaResposta, 2),
                ExposicaoMaisVisitada = nomeExposicaoMaisVisitada
            };
        }
    }
}
