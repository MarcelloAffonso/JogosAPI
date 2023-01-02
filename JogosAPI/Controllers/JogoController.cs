using AutoMapper;
using JogosAPI.Data;
using JogosAPI.Data.DTOs;
using JogosAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
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
    public IEnumerable<ReadJogoDTO> RecuperaJogos([FromQuery]int skip = 0, [FromQuery]int take = 50)
    {
        // Passa os parametros skip e take para permitir a paginação da resposta
        return _mapper.Map<List<ReadJogoDTO>>(_context.Jogos.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaJogoPorId(int id)
    {
        // Caso o jogo não seja encontrado, retorna um erro 404 (Not Found)
        var jogo = _context.Jogos.FirstOrDefault(jogo => jogo.Id == id);
        if (jogo == null) return NotFound();

        var jogoDTO = _mapper.Map<ReadJogoDTO>(jogo);

        return Ok(jogoDTO);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaJogo(int id, [FromBody] 
        UpdateJogoDTO jogoDTO)
    {
        var jogo = _context.Jogos.FirstOrDefault(jogo => jogo.Id == id);
        
        // Se o jogo procurado não existe, devolve um 404 Not Found
        if (jogo == null) return NotFound();

        // usa o Auto Mapper para passar os dados do DTO para o o objeto do jogo
        _mapper.Map(jogoDTO, jogo);
        _context.SaveChanges();

        // Em atualizações, normalmente devolve um No Content
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaJogoParcial(int id, JsonPatchDocument<UpdateJogoDTO> patch)
    {
        var jogo = _context.Jogos.FirstOrDefault(jogo => jogo.Id == id);

        // Se o jogo procurado não existe, devolve um 404 Not Found
        if (jogo == null) return NotFound();

        var jogoParaAtualizar = _mapper.Map<UpdateJogoDTO>(jogo);

        patch.ApplyTo(jogoParaAtualizar, ModelState);

        // Se não conseguiu validar o modelo do Jogo, descarta e retorna um Validation problem (erro de validação) para o cliente
        if(!TryValidateModel(jogoParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        // usa o Auto Mapper para passar os dados do DTO para o o objeto do jogo
        _mapper.Map(jogoParaAtualizar, jogo);
        _context.SaveChanges();

        // Em atualizações, normalmente devolve um No Content
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaJogo(int id)
    {
        var jogo = _context.Jogos.FirstOrDefault(jogo => jogo.Id == id);

        // Se o jogo procurado não existe, devolve um 404 Not Found
        if (jogo == null) return NotFound();

        _context.Remove(jogo);
        _context.SaveChanges();

        return NoContent();

    }
}
