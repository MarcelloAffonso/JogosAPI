using AutoMapper;
using JogosAPI.Data.DTOs;
using JogosAPI.Models;

namespace JogosAPI.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        // Cria um mapeamento do DTO para a classe de Jogo
        CreateMap<CreateJogoDTO, Jogo>();
    }
}
