using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarsMuseumAPI.Models
{
    public class Exposicao
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; }

        public int OrdemExibicao { get; set; }

        // Navegação
        public virtual ICollection<VisitaExposicao> VisitasExposicao { get; set; }
    }
}