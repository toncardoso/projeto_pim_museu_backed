using System;
using System.ComponentModel.DataAnnotations;

namespace MarsMuseumAPI.Models
{
    public class LogSistema
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Mensagem { get; set; }

        [Required]
        [StringLength(20)]
        public string Nivel { get; set; }

        [StringLength(100)]
        public string Origem { get; set; }

        [Required]
        public DateTime DataHora { get; set; } = DateTime.Now;
    }
}