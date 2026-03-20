using System;
using System.ComponentModel.DataAnnotations;

namespace MarsMuseumAPI.Models
{
    public class Pesquisa
    {
        public int Id { get; set; }

        public int IdVisitante { get; set; }

        [Required]
        [StringLength(20)]
        public string ExperienciaGeral { get; set; }

        [Required]
        [StringLength(20)]
        public string QualidadeObras { get; set; }

        [Required]
        [StringLength(20)]
        public string ClarezaInformacoes { get; set; }

        [Required]
        [StringLength(20)]
        public string Recomendaria { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string Sugestoes { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        // Navegação
        public virtual Visitante Visitante { get; set; }
    }
}