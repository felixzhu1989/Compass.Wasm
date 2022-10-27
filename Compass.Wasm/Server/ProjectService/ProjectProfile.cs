using AutoMapper;
using Compass.ProjectService.Domain.Entities;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.ProjectService;

public class ProjectProfile:Profile
{
    public ProjectProfile()
    {
        CreateMap<Project,ProjectResponse>();/*
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => $"{src.Id}"))
            .ForMember(dest => dest.OdpNumber, opt => opt.MapFrom(src => $"{src.OdpNumber}"))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Name}"))
            .ForMember(dest => dest.ProjectType, opt => opt.MapFrom(src => $"{src.ProjectType}"))
            .ForMember(dest => dest.RiskLevel, opt => opt.MapFrom(src => $"{src.RiskLevel}"))
            .ForMember(dest => dest.SpecialNotes, opt => opt.MapFrom(src => $"{src.SpecialNotes}"))
            .ForMember(dest => dest.ContractUrl, opt => opt.MapFrom(src => $"{src.ContractUrl}"))
            .ForMember(dest => dest.BomUrl, opt => opt.MapFrom(src => $"{src.BomUrl}"))
            .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => $"{src.CreationTime}"));*/
        CreateMap<Drawing, DrawingResponse>();




    }
}