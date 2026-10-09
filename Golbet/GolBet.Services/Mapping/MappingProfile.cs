// GolBet.Services/Mapping/MappingProfile.cs
using AutoMapper;
using GolBet.Entities;
using GolBet.Services.DTOs;

namespace GolBet.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // El aplanamiento extrae automáticamente Match.HomeTeam.Name hacia MatchDto.HomeTeamName
        CreateMap<Match, MatchDto>();

        // Mapeo explícito para el detalle del partido
        CreateMap<Match, MatchDetailDto>()
            .ForMember(dto => dto.TotalBets,
                options => options.MapFrom(match => match.Bets.Count));

        // Mapas para Equipos
        CreateMap<Team, TeamDto>();
        CreateMap<TeamFormDto, Team>().ReverseMap();
        CreateMap<MatchFormDto, Match>().ReverseMap();

    }
}