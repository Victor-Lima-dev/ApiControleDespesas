using ApiControleDespesas.Context;
using ApiControleDespesas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiControleDespesas.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReceitasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReceitasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receita>>> GetReceitas()
        {
             var lista = await _context.Receitas.ToListAsync();
            return Ok(lista);
        }

        [HttpPost]

        public async Task<ActionResult<Receita>> PostReceita(Receita receita)
        {
            var verificacaoDescricao = _context.Receitas.Any(x => x.Descricao == receita.Descricao);
            var verificacaoData = _context.Receitas.Any(c => c.Data.Month == receita.Data.Month);
          
         if (verificacaoData && verificacaoDescricao)
           {
                return BadRequest();
            }

          else
           {
                _context.Receitas.Add(receita);
                await _context.SaveChangesAsync();
                return Ok();
            }            
        }



    }
}
