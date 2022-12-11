using ApiControleDespesas.Context;
using ApiControleDespesas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiControleDespesas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ResumoController(AppDbContext context)
        {
            _context = context;
        }

        //metodo para mostrar o total do mes
        [HttpGet("{ano}/{mes}")]
        public async Task<ActionResult<Resumo>> GetTotalMes(int ano, int mes)
        {
            var totalReceitasMes = await _context.Receitas.Where(x => x.Data.Month == mes && x.Data.Year == ano).SumAsync(x => x.Valor);
            var totalDespesasMes = await _context.Despesas.Where(x => x.Data.Month == mes && x.Data.Year == ano).SumAsync(x => x.Valor);
            var saldoMes = totalReceitasMes - totalDespesasMes;

            var totalAlimentacao = await _context.Despesas.Where(x => x.Data.Month == mes && x.Data.Year == ano && x.Categoria == "alimentação").SumAsync(x => x.Valor);
            var totalTransporte = await _context.Despesas.Where(x => x.Data.Month == mes && x.Data.Year == ano && x.Categoria == "transporte").SumAsync(x => x.Valor);
            var totalMoradia = await _context.Despesas.Where(x => x.Data.Month == mes && x.Data.Year == ano && x.Categoria == "moradia").SumAsync(x => x.Valor);
            var totalEducacao = await _context.Despesas.Where(x => x.Data.Month == mes && x.Data.Year == ano && x.Categoria == "educação").SumAsync(x => x.Valor);
            var totalSaude = await _context.Despesas.Where(x => x.Data.Month == mes && x.Data.Year == ano && x.Categoria == "saúde").SumAsync(x => x.Valor);
            var totalLazer = await _context.Despesas.Where(x => x.Data.Month == mes && x.Data.Year == ano && x.Categoria == "lazer").SumAsync(x => x.Valor);
            var totalOutros = await _context.Despesas.Where(x => x.Data.Month == mes && x.Data.Year == ano && x.Categoria == "outros").SumAsync(x => x.Valor);

            var resumo = new Resumo
            {
                TotalReceitasMes = totalReceitasMes,
                TotalDespesasMes = totalDespesasMes,
                SaldoMes = saldoMes,
                TotalAlimentacao = totalAlimentacao,
                TotalTransporte = totalTransporte,
                TotalMoradia = totalMoradia,
                TotalEducacao = totalEducacao,
                TotalSaude = totalSaude,
                TotalLazer = totalLazer,
                TotalOutros = totalOutros
            };

            return Ok(resumo);

        }
    }
}
