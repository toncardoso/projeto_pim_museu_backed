using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarsMuseumAPI.Data;
using MarsMuseumAPI.Models;

namespace MarsMuseumAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de visitantes do museu
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VisitantesController : ControllerBase
    {
        private readonly MarsMuseumContext _context;

        public VisitantesController(MarsMuseumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os visitantes registrados
        /// </summary>
        /// <returns>Lista de todos os visitantes</returns>
        /// <response code="200">Retorna a lista de visitantes</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Visitante>>> GetVisitantes()
        {
            return await _context.Visitantes.ToListAsync();
        }

        /// <summary>
        /// Obtém um visitante específico pelo ID
        /// </summary>
        /// <param name="id">ID do visitante</param>
        /// <returns>Os dados do visitante solicitado</returns>
        /// <response code="200">Retorna o visitante solicitado</response>
        /// <response code="404">Se o visitante não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Visitante>> GetVisitante(int id)
        {
            var visitante = await _context.Visitantes.FindAsync(id);

            if (visitante == null)
            {
                return NotFound();
            }

            return visitante;
        }

        /// <summary>
        /// Registra um novo visitante no sistema
        /// </summary>
        /// <param name="visitante">Dados do visitante a ser registrado</param>
        /// <returns>O visitante criado com seu ID</returns>
        /// <response code="201">Retorna o visitante recém-criado</response>
        /// <response code="400">Se os dados do visitante forem inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Visitante>> PostVisitante(Visitante visitante)
        {
            visitante.DataHoraRegistro = DateTime.Now;
            visitante.PesquisaCompleta = false;
            
            _context.Visitantes.Add(visitante);
            await _context.SaveChangesAsync();

            // Registrar log
            _context.LogsSistema.Add(new LogSistema
            {
                Mensagem = $"Novo visitante registrado: {visitante.Nome}",
                Nivel = "Informação",
                DataHora = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVisitante", new { id = visitante.Id }, visitante);
        }
    }
}
