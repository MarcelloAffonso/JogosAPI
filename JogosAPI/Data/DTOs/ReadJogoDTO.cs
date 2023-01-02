using System.ComponentModel.DataAnnotations;

namespace JogosAPI.Data.DTOs;

public class ReadJogoDTO
{
    public string Titulo { get; set; }

    public string Genero { get; set; }

    public DateTime DataLancamento { get; set; }

    public string Descricao { get; set; }

    public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}
