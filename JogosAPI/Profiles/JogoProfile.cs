using AutoMapper;
using JogosAPI.Data.DTOs;
using JogosAPI.Models;

namespace JogosAPI.Profiles;

public class JogoProfile : Profile
{
    public JogoProfile()
    {
        // Cria um mapeamento dos DTO para a classe de Jogo
        CreateMap<CreateJogoDTO, Jogo>();
        CreateMap<UpdateJogoDTO, Jogo>();
        CreateMap<Jogo, UpdateJogoDTO>();
        CreateMap<Jogo, ReadJogoDTO>();
    }
}
