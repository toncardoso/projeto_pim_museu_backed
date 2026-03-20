using System;
using System.ComponentModel.DataAnnotations;

namespace MarsMuseumAPI.Models
{
    public class PalavraChave
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Palavra { get; set; }

        public int Frequencia { get; set; }

        public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;
    }
}