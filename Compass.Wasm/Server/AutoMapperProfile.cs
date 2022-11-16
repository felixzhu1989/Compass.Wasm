using AutoMapper;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.IdentityService;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User,UserResponse>();

        #region ProjectService
        CreateMap<Project, ProjectResponse>();
        //.ForMember(dest => dest.OdpNumber, opt => opt.MapFrom(src => $"{src.OdpNumber}"))
        CreateMap<Drawing, DrawingResponse>();
        CreateMap<Compass.ProjectService.Domain.Entities.Module, ModuleResponse>();
        CreateMap<DrawingPlan, DrawingPlanResponse>();
        CreateMap<Tracking, TrackingResponse>();
        CreateMap<Problem, ProblemResponse>();

        #endregion

        #region CategoryService
        CreateMap<Product, ProductResponse>();
        CreateMap<Model, ModelResponse>();
        CreateMap<ProblemType, ProblemTypeResponse>();
        #endregion
    }
}