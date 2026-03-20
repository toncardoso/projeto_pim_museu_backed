using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarsMuseumAPI.Data;
using MarsMuseumAPI.Models;

namespace MarsMuseumAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de exposições do museu
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExposicoesController : ControllerBase
    {
        private readonly MarsMuseumContext _context;

        public ExposicoesController(MarsMuseumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as exposições do museu
        /// </summary>
        /// <returns>Lista de todas as exposições</returns>
        /// <response code="200">Retorna a lista de exposições</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Exposicao>>> GetExposicoes()
        {
            return await _context.Exposicoes
                .OrderBy(e => e.OrdemExibicao)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém uma exposição específica pelo ID
        /// </summary>
        /// <param name="id">ID da exposição</param>
        /// <returns>Os dados da exposição solicitada</returns>
        /// <response code="200">Retorna a exposição solicitada</response>
        /// <response code="404">Se a exposição não for encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Exposicao>> GetExposicao(int id)
        {
            var exposicao = await _context.Exposicoes.FindAsync(id);

            if (exposicao == null)
            {
                return NotFound();
            }

            return exposicao;
        }

        /// <summary>
        /// Obtém estatísticas de visitação para uma exposição específica
        /// </summary>
        /// <param name="id">ID da exposição</param>
        /// <returns>Estatísticas de visitação da exposição</returns>
        /// <response code="200">Retorna as estatísticas da exposição</response>
        /// <response code="404">Se a exposição não for encontrada</response>
        [HttpGet("{id}/estatisticas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetEstatisticasExposicao(int id)
        {
            var exposicao = await _context.Exposicoes.FindAsync(id);
            if (exposicao == null)
            {
                return NotFound();
            }

            var totalVisitas = await _context.VisitasExposicao
                .Where(v => v.IdExposicao == id)
                .CountAsync();

            var tempoMedio = await _context.VisitasExposicao
                .Where(v => v.IdExposicao == id)
                .Select(v => (double?)v.DuracaoSegundos)
                .AverageAsync() ?? 0;

            return new
            {
                Exposicao = exposicao.Titulo,
                TotalVisitas = totalVisitas,
                TempoMedioSegundos = tempoMedio,
                TempoMedioFormatado = TimeSpan.FromSeconds(tempoMedio).ToString(@"mm\:ss")
            };
        }
    }
}
