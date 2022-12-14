using AutoMapper;
using Compass.PlanService.Domain.Entities;
using Compass.QualityService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.IdentityService;
using Compass.Wasm.Shared.PlanService;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Shared.QualityService;

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
        CreateMap<Issue, IssueResponse>();
        #endregion

        #region CategoryService
        CreateMap<Product, ProductResponse>();
        CreateMap<Model, ModelResponse>();
        CreateMap<ModelType, ModelTypeResponse>();
        CreateMap<ProblemType, ProblemTypeResponse>();
        #endregion

        #region PlanService
        CreateMap<ProductionPlan, ProductionPlanResponse>();
        #endregion

        #region QualityService
        CreateMap<FinalInspectionCheckItemType, FinalInspectionCheckItemTypeResponse>();


        #endregion

    }
}