using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using DFC.App.Account.Application.SkillsHealthCheck.Models;
using DFC.App.Account.Services.SHC.Models;

namespace DFC.App.Account.Services.SHC.Services
{
    [ExcludeFromCodeCoverage]
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SkillsDocument, ShcDocument>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedAt)))
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.DocumentId))
                .ConstructUsing(dest => new ShcDocument());

        }
    }
}
