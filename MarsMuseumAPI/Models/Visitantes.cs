using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarsMuseumAPI.Models
{
    public class Visitante
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public DateTime DataVisita { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public bool ExposicaoCompleta { get; set; }

        public bool PesquisaCompleta { get; set; }

        // Navegação
        public virtual ICollection<VisitaExposicao> VisitasExposicao { get; set; }
        public virtual Pesquisa Pesquisa { get; set; }
    }
}
