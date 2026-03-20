using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarsMuseumAPI.Data;
using MarsMuseumAPI.Models;

namespace MarsMuseumAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de pesquisas de satisfação
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PesquisasController : ControllerBase
    {
        private readonly MarsMuseumContext _context;

        public PesquisasController(MarsMuseumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registra uma nova pesquisa de satisfação
        /// </summary>
        /// <param name="pesquisa">Dados da pesquisa a ser registrada</param>
        /// <returns>A pesquisa criada com seu ID</returns>
        /// <response code="201">Retorna a pesquisa recém-criada</response>
        /// <response code="400">Se os dados da pesquisa forem inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pesquisa>> PostPesquisa(Pesquisa pesquisa)
        {
            pesquisa.DataHora = DateTime.Now;
            _context.Pesquisas.Add(pesquisa);
            
            // Marcar visitante como tendo completado a pesquisa
            var visitante = await _context.Visitantes.FindAsync(pesquisa.IdVisitante);
            if (visitante != null)
            {
                visitante.PesquisaCompleta = true;
            }
            
            // Processar palavras-chave das sugestões
            if (!string.IsNullOrEmpty(pesquisa.Sugestoes))
            {
                var palavras = pesquisa.Sugestoes.Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '\n', '\r' },
                    StringSplitOptions.RemoveEmptyEntries)
                    .Where(p => p.Length > 3)
                    .Select(p => p.ToLower());
                
                foreach (var palavra in palavras)
                {
                    var palavraChave = await _context.PalavrasChave.FirstOrDefaultAsync(p => p.Palavra == palavra);
                    if (palavraChave != null)
                    {
                        palavraChave.Frequencia++;
                        palavraChave.UltimaAtualizacao = DateTime.Now;
                    }
                    else
                    {
                        _context.PalavrasChave.Add(new PalavraChave { 
                            Palavra = palavra, 
                            Frequencia = 1,
                            UltimaAtualizacao = DateTime.Now
                        });
                    }
                }
            }
            
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPesquisa", new { id = pesquisa.Id }, pesquisa);
        }

        /// <summary>
        /// Obtém uma pesquisa específica pelo ID
        /// </summary>
        /// <param name="id">ID da pesquisa</param>
        /// <returns>Os dados da pesquisa solicitada</returns>
        /// <response code="200">Retorna a pesquisa solicitada</response>
        /// <response code="404">Se a pesquisa não for encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pesquisa>> GetPesquisa(int id)
        {
            var pesquisa = await _context.Pesquisas.FindAsync(id);
            
            if (pesquisa == null)
            {
                return NotFound();
            }
            
            return pesquisa;
        }
    }
}
