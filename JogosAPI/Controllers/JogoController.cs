using AutoMapper;
using JogosAPI.Data;
using JogosAPI.Data.DTOs;
using JogosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JogosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class JogoController : ControllerBase
{
    private JogoContext _context;
    private IMapper _mapper;

    public JogoController(JogoContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionaJogo(
        [FromBody] CreateJogoDTO jogoDto)
    {
        // Mapeia e faz a "transferencia" dos dados do DTO para o objeto
        Jogo jogo = _mapper.Map<Jogo>(jogoDto);
        _context.Jogos.Add(jogo);
        _context.SaveChanges();

        // Por padrão, um post deverá gravar o objeto passado e depois devolve-lo para o cliente
        return CreatedAtAction(nameof(RecuperaJogoPorId), new { id = jogo.Id },
            jogoDto);
    }

    [HttpGet]
    public IEnumerable<Jogo> RecuperaJogos([FromQuery]int skip = 0, [FromQuery]int take = 50)
    {
        // Passa os parametros skip e take para permitir a paginação da resposta
        return _context.Jogos.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaJogoPorId(int id)
    {
        // Caso o jogo não seja encontrado, retorna um erro 404 (Not Found)
        var jogo = _context.Jogos.FirstOrDefault(jogo => jogo.Id == id);
        if (jogo == null) return NotFound();

        return Ok(jogo);
    }
}
