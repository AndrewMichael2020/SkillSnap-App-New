using AutoMapper;
using SkillSnap.Api.Models;
using SkillSnap.Shared;

namespace SkillSnap.Api.Mapping
{
    public class SkillSnapProfile : Profile
    {
        public SkillSnapProfile()
        {
            CreateMap<PortfolioUser, PortfolioUserDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Skill, SkillDto>().ReverseMap();
        }
    }
}
