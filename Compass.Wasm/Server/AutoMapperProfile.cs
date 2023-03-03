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
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<Drawing, DrawingDto>().ReverseMap();
        CreateMap<Compass.ProjectService.Domain.Entities.Module, ModuleDto>().ReverseMap();



        CreateMap<DrawingPlan, DrawingPlanResponse>().ReverseMap();
        CreateMap<Tracking, TrackingResponse>().ReverseMap();
        CreateMap<Problem, ProblemResponse>().ReverseMap();
        CreateMap<Issue, IssueResponse>().ReverseMap();
        #endregion

        #region CategoryService
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Model, ModelDto>().ReverseMap();
        CreateMap<ModelType, ModelTypeDto>().ReverseMap();
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