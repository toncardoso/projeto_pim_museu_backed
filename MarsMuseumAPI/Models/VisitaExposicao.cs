using System;
using System.ComponentModel.DataAnnotations;

namespace MarsMuseumAPI.Models
{
    public class VisitaExposicao
    {
        public int Id { get; set; }

        public int IdVisitante { get; set; }

        public int IdExposicao { get; set; }

        public int DuracaoSegundos { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        // Navegação
        public virtual Visitante Visitante { get; set; }
        public virtual Exposicao Exposicao { get; set; }
    }
}