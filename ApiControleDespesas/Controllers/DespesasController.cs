using ApiControleDespesas.Context;
using ApiControleDespesas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiControleDespesas.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DespesasController : ControllerBase
    {
        //banco de dados
        private readonly AppDbContext _context;
        //construtor
        public DespesasController(AppDbContext context)
        {
            _context = context;
        }

        //metodo para listar despesas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Despesa>>> GetDespesas()
        {
            var lista = await _context.Despesas.ToListAsync();
            return Ok(lista);
        }

        //metodo para criar despesa
        [HttpPost]
        public async Task<ActionResult<Despesa>> PostDespesa(Despesa despesa)


        {
            var verificaCategoria = despesa.Categoria;

            if (verificaCategoria == null)
            {
                despesa.Categoria = "Outros";
            }
         

            
            var verificacaoDescricao =  _context.Despesas.Any(x => x.Descricao == despesa.Descricao);
            var verificacaoData = _context.Despesas.Any(c => c.Data.Month == despesa.Data.Month);

            if (verificacaoData && verificacaoDescricao)
            {
                return BadRequest();
            }

            else
            {
                _context.Despesas.Add(despesa);
                await _context.SaveChangesAsync();
                return Ok();
            }


    }

        //metodo para buscar despesa por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Despesa>> GetDespesa(int id)
        {
            var despesa = await _context.Despesas.FindAsync(id);
            if (despesa == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(despesa);
            }
        }




        //metodo para editar despesa
        [HttpPut("{id}")]
        public async Task<ActionResult<Despesa>> EditDespesa(int id, Despesa despesa)
        {
            if (id != despesa.DespesaId)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(despesa).State = EntityState.Modified;
                return Ok(despesa);
            }
        }


        //metodo para deletar despesa
        [HttpDelete("{id}")]
        public async Task<ActionResult<Despesa>> DeleteDespesa(int id)
        {
            var despesa = await _context.Despesas.FindAsync(id);
            if (despesa == null)
            {
                return NotFound();
            }
            else
            {
                _context.Despesas.Remove(despesa);
                await _context.SaveChangesAsync();
                return Ok(despesa);
            }
        }
    }

}
