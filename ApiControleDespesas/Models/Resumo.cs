namespace ApiControleDespesas.Models
{
    public class Resumo
    {
        public decimal TotalReceitasMes { get; set; }
        public decimal TotalDespesasMes { get; set; }
        public decimal SaldoMes { get; set; }
        
        public decimal TotalAlimentacao { get; set; }
        public decimal TotalTransporte { get; set; }
        public decimal TotalMoradia { get; set; }
        public decimal TotalEducacao { get; set; }
        public decimal TotalSaude { get; set; }
        public decimal TotalLazer { get; set; }
        public decimal TotalOutros { get; set; }
        

    }
}
