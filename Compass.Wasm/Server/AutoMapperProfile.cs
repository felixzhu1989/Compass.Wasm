using AutoMapper;
using Compass.PlanService.Domain.Entities;
using Compass.QualityService.Domain.Entities;
using Compass.TodoService.Domain.Entities;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Identities;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared.Quality;
using Compass.Wasm.Shared.Todos;

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
        CreateMap<CutList, CutListDto>().ReverseMap();


        CreateMap<DrawingPlan, DrawingPlanResponse>().ReverseMap();


        
        CreateMap<Lesson, LessonDto>().ReverseMap();
        #endregion

        #region CategoryService
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Model, ModelDto>().ReverseMap();
        CreateMap<ModelType, ModelTypeDto>().ReverseMap();
        CreateMap<ProblemType, ProblemTypeResponse>().ReverseMap();
        #endregion

        #region PlanService
        CreateMap<MainPlan, MainPlanDto>().ReverseMap();
        CreateMap<Issue, IssueDto>().ReverseMap();


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