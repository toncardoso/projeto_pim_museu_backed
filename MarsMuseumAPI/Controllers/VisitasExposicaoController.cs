using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarsMuseumAPI.Data;
using MarsMuseumAPI.Models;

namespace MarsMuseumAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de visitas às exposições
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VisitasExposicaoController : ControllerBase
    {
        private readonly MarsMuseumContext _context;

        public VisitasExposicaoController(MarsMuseumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registra uma nova visita a uma exposição
        /// </summary>
        /// <param name="visitaExposicao">Dados da visita a ser registrada</param>
        /// <returns>A visita criada com seu ID</returns>
        /// <response code="201">Retorna a visita recém-registrada</response>
        /// <response code="400">Se os dados da visita forem inválidos</response>
        /// <response code="404">Se o visitante ou exposição não forem encontrados</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VisitaExposicao>> PostVisitaExposicao(VisitaExposicao visitaExposicao)
        {
            // Verificar se o visitante existe
            var visitante = await _context.Visitantes.FindAsync(visitaExposicao.IdVisitante);
            if (visitante == null)
            {
                return NotFound("Visitante não encontrado");
            }

            // Verificar se a exposição existe
            var exposicao = await _context.Exposicoes.FindAsync(visitaExposicao.IdExposicao);
            if (exposicao == null)
            {
                return NotFound("Exposição não encontrada");
            }

            visitaExposicao.DataHora = DateTime.Now;
            _context.VisitasExposicao.Add(visitaExposicao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVisitaExposicao", new { id = visitaExposicao.Id }, visitaExposicao);
        }

        /// <summary>
        /// Obtém uma visita específica pelo ID
        /// </summary>
        /// <param name="id">ID da visita</param>
        /// <returns>Os dados da visita solicitada</returns>
        /// <response code="200">Retorna a visita solicitada</response>
        /// <response code="404">Se a visita não for encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VisitaExposicao>> GetVisitaExposicao(int id)
        {
            var visitaExposicao = await _context.VisitasExposicao
                .Include(v => v.Visitante)
                .Include(v => v.Exposicao)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visitaExposicao == null)
            {
                return NotFound();
            }

            return visitaExposicao;
        }

        /// <summary>
        /// Obtém todas as visitas registradas
        /// </summary>
        /// <returns>Lista de todas as visitas às exposições</returns>
        /// <response code="200">Retorna a lista de visitas</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VisitaExposicao>>> GetVisitasExposicao()
        {
            return await _context.VisitasExposicao
                .Include(v => v.Visitante)
                .Include(v => v.Exposicao)
                .ToListAsync();
        }
    }
}
