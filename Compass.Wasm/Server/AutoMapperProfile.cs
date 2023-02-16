using AutoMapper;
using Compass.PlanService.Domain.Entities;
using Compass.QualityService.Domain.Entities;
using Compass.TodoService.Domain.Entities;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.IdentityService;
using Compass.Wasm.Shared.PlanService;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Shared.QualityService;
using Compass.Wasm.Shared.TodoService;

namespace Compass.Wasm.Server;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User,UserDto>().ReverseMap();

        #region ProjectService
        CreateMap<Project, ProjectResponse>().ReverseMap();
        //.ForMember(dest => dest.OdpNumber, opt => opt.MapFrom(src => $"{src.OdpNumber}"))
        CreateMap<Drawing, DrawingResponse>().ReverseMap();
        CreateMap<Compass.ProjectService.Domain.Entities.Module, ModuleResponse>().ReverseMap();
        CreateMap<DrawingPlan, DrawingPlanResponse>().ReverseMap();
        CreateMap<Tracking, TrackingResponse>().ReverseMap();
        CreateMap<Problem, ProblemResponse>().ReverseMap();
        CreateMap<Issue, IssueResponse>().ReverseMap();
        #endregion

        #region CategoryService
        CreateMap<Product, ProductResponse>().ReverseMap();
        CreateMap<Model, ModelResponse>().ReverseMap();
        CreateMap<ModelType, ModelTypeResponse>().ReverseMap();
        CreateMap<ProblemType, ProblemTypeResponse>().ReverseMap();
        #endregion

        #region PlanService
        CreateMap<ProductionPlan, ProductionPlanResponse>().ReverseMap();
        #endregion

        #region QualityService
        CreateMap<FinalInspectionCheckItemType, FinalInspectionCheckItemTypeResponse>().ReverseMap();


        #endregion

        #region TodoService
        CreateMap<Todo, TodoDto>().ReverseMap();
        CreateMap<Memo, MemoDto>().ReverseMap();
        #endregion

    }
}