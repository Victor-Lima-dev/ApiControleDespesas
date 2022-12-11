using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiControleDespesas.Models
{
    public class Receita
    {
        public int ReceitaId { get; set; }
        [Required]
        public string Descricao { get; set; }        
        [Required]
        [Column(TypeName = "decimal(10,2)")]

        public decimal Valor { get; set; }
        [Required]
        public DateTime Data { get; set; }

    }
}
