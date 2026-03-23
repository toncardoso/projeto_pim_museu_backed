using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual Visitante Visitante { get; set; }

        [JsonIgnore]
        public virtual Exposicao Exposicao { get; set; }
    }
}
