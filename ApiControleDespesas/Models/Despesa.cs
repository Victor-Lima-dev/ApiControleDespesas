using System.ComponentModel.DataAnnotations;

namespace ApiControleDespesas.Models
{
    public class Despesa
    {
        public int DespesaId { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public DateTime Data { get; set; }
    }
}
