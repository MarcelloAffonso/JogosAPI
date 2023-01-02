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

    /// <summary>
    /// Adiciona um novo jogo ao banco de dados.
    /// </summary>
    /// <param name="jogoDto">Objeto com os campos necessários para a criação de um jogo</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
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

    /// <summary>
    /// Faz uma consulta no banco de dados por todos os jogos em determinado range.
    /// </summary>
    /// <param name="skip">Quantidade de jogos que serão "pulados" (usado para paginação).</param>
    /// <param name="take">Quantidade de jogos que serão "capturados" (usado para paginação).</param>
    [HttpGet]
    public IEnumerable<ReadJogoDTO> RecuperaJogos([FromQuery]int skip = 0, [FromQuery]int take = 50)
    {
        // Passa os parametros skip e take para permitir a paginação da resposta
        return _mapper.Map<List<ReadJogoDTO>>(_context.Jogos.Skip(skip).Take(take));
    }

    /// <summary>
    /// Faz uma consulta no banco de dados para buscar por um jogo específico.
    /// </summary>
    /// <param name="id">Id do jogo que deverá ser recuperado.</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso o jogo exista no banco de dados.</response>
    /// <response code="404">Caso o jogo não exista exista no banco de dados.</response>
    [HttpGet("{id}")]
    public IActionResult RecuperaJogoPorId(int id)
    {
        // Caso o jogo não seja encontrado, retorna um erro 404 (Not Found)
        var jogo = _context.Jogos.FirstOrDefault(jogo => jogo.Id == id);
        if (jogo == null) return NotFound();

        var jogoDTO = _mapper.Map<ReadJogoDTO>(jogo);

        return Ok(jogoDTO);
    }

    /// <summary>
    /// Atualiza o jogo com o id informado no banco de dados.
    /// </summary>
    /// <param name="id">Identificador do jogo que deverá ter seus dados atualziados</param>
    /// <param name="jogoDTO">Objeto com os campos necessários para aatualização do jogo no banco de dados</param>
    /// <returns></returns>
    /// <response code="204">Caso o jogo tenha sido encontrado e atualizado no banco de dados.</response>
    /// <response code="404">Caso o jogo não exista exista no banco de dados.</response>
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

    /// <summary>
    /// Atualiza parcialmente o jogo com o id informado no banco de dados.
    /// </summary>
    /// <param name="id">Identificador do jogo quye deverá ser parcialmente alterado</param>
    /// <param name="patch">JSON que contém os dados que deverão ser atualizados no jogo que possui o ID informado</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso o jogo tenha sido encontrado e atualizado parcialmente no banco de dados.</response>
    /// <response code="404">Caso o jogo não exista exista no banco de dados.</response>
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

    /// <summary>
    /// Remove o jogo que possui o Id informado do banco de dados
    /// </summary>
    /// <param name="id">Identificador do jogo que deverá ser removido do banco de dados</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso o jogo tenha sido encontrado e removido do banco de dados.</response>
    /// <response code="404">Caso o jogo não exista exista no banco de dados.</response>
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
