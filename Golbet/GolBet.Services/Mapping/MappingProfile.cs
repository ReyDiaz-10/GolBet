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
    }
}