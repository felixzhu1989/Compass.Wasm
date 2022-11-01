using AutoMapper;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region ProjectService
        CreateMap<Project, ProjectResponse>();
        //.ForMember(dest => dest.OdpNumber, opt => opt.MapFrom(src => $"{src.OdpNumber}"))
        CreateMap<Drawing, DrawingResponse>();

        #endregion

        #region CategoryService
        CreateMap<Product, ProductResponse>();
        CreateMap<Model, ModelResponse>();
        #endregion
    }
}