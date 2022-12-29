using JogosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JogosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class JogoController : ControllerBase
{
    private static List<Jogo> jogos = new List<Jogo>();

    private static int id = 0;

    [HttpPost]
    public IActionResult AdicionaJogo([FromBody] Jogo jogo)
    {
        jogo.Id = id++;
        jogos.Add(jogo);

        // Por padrão, um post deverá gravar o objeto passado e depois devolve-lo para o cliente
        return CreatedAtAction(nameof(RecuperaJogoPorId), new { id = jogo.Id },
            jogo);
    }

    [HttpGet]
    public IEnumerable<Jogo> RecuperaJogos([FromQuery]int skip = 0, [FromQuery]int take = 50)
    {
        // Passa os parametros skip e take para permitir a paginação da resposta
        return jogos.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaJogoPorId(int id)
    {
        // Caso o jogo não seja encontrado, retorna um erro 404 (Not Found)
        var jogo = jogos.FirstOrDefault(jogo => jogo.Id == id);
        if (jogo == null) return NotFound();

        return Ok(jogo);
    }
}
