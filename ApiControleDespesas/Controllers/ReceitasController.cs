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

        //metodo para listar receita
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receita>>> GetReceitas()
        {
            var lista = await _context.Receitas.ToListAsync();
            return Ok(lista);
        }

        //metodo para categorizar por descricao
        //Para lembrar do código, para fazer uma pesquisa nós fazemos um metodo que irá receber uma string
        //ai usamos o Where para fazer a pesquisa utilizando o Contains para ver se aquela string bate em alguma coisa
        //dentro do banco de dados e retornar a lista de acordo com a pesquisa feita pelo usuário.

        //no Http o descricao é o nome que vai aparecer na url ele é constante e o {descricao} é qual descricao a gente vai
        //procurar, portanto ela é de acordo com a variavel que entra no metodo.

        [HttpGet("descricao/{descricao}")]
        public async Task<ActionResult<IEnumerable<Receita>>> GetReceitasPorDescricao(string descricao)
        {
            var lista = await _context.Receitas.Where(x => x.Descricao.Contains(descricao)).ToListAsync();
            return Ok(lista);
        }

        //metodo para categorizar por data
        [HttpGet("{ano}/{mes}")]
        public async Task<ActionResult<IEnumerable<Receita>>> GetReceitasPorData(int ano, int mes)
        {
            var lista = await _context.Receitas.Where(x => x.Data.Month == mes && x.Data.Year == ano).ToListAsync();
            return Ok(lista);
        }

        //metodo para adicionar receita
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

        //metodo para editar receita
        [HttpPut("{id}")]
        public async Task<ActionResult<Receita>> EditReceita(int id, Receita receita)
        {
            if (id != receita.ReceitaId)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(receita).State = EntityState.Modified;
                return Ok(receita);
            }    
    
        }

        //metodo para detalhar receita
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Receita>> Get(int id)
        {
            var receita = _context.Receitas.FirstOrDefault(x => x.ReceitaId == id);

            
            if (receita == null)
            {
                return NotFound("Receita nao encontrada");
            }
            
            return receita;
        }

        //metodo para deletar receita
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Receita>> Delete(int id)
        {
            var receita = _context.Receitas.FirstOrDefault(x => x.ReceitaId == id);

            if (receita == null)
            {
                return NotFound();
            }

            _context.Receitas.Remove(receita);
            await _context.SaveChangesAsync(); 

            return Ok(receita);
        }


        
    }
}
