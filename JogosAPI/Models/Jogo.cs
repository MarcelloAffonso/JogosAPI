using System.ComponentModel.DataAnnotations;

namespace JogosAPI.Models;

public class Jogo
{
    [Key]
    [Required]
    public int Id { get; set; } 

    [Required(ErrorMessage ="O título do jogo é obrigatório!")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O genero do jogo é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O tamanho máximo do gênero é de 50 caracteres e não pode ser excedido!")]
    public string Genero { get; set; }

    [Required(ErrorMessage = "A data de lançamento do jogo é obrigatória!")]
    public DateTime DataLancamento { get; set; }

    [Required(ErrorMessage ="A descrição do jogo é obrigatória!")]
    public string Descricao { get; set; }
}
