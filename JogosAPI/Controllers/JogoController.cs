using JogosAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace JogosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class JogoController : ControllerBase
{
    private static List<Jogo> jogos = new List<Jogo>();

    [HttpPost]
    public void AdicionaJogo([FromBody] Jogo jogo)
    {
        jogos.Add(jogo);
        Console.WriteLine(jogo.Titulo);
        Console.WriteLine(jogo.Descricao);
        Console.WriteLine(jogo.Genero);
        Console.WriteLine(jogo.DataLancamento.ToString());
    }
}
