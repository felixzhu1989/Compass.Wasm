using Compass.DataService.Domain;
using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data;

namespace Compass.Wasm.Server.Services.Data;
public interface IModuleDataService : IBaseService<ModuleData>
{
}
public class ModuleDataService : IModuleDataService
{
    private readonly DataDomainService _domainService;
    private readonly DataDbContext _dataDbContext;
    private readonly IDataRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly IEventBus _eventBus;

    public ModuleDataService(DataDomainService domainService, DataDbContext dataDbContext, IDataRepository repository, IProjectRepository projectRepository, IEventBus eventBus)
    {
        _domainService = domainService;
        _dataDbContext = dataDbContext;
        _repository = repository;
        _projectRepository = projectRepository;
        _eventBus = eventBus;
    }

    #region 增删改查
    public async Task<ApiResponse<List<ModuleData>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetModulesDataAsync();
            var orderModels = models.OrderByDescending(x => x.CreationTime).ToList();
            return new ApiResponse<List<ModuleData>> { Status = true, Result = orderModels };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ModuleData>> { Status = false, Message = e.Message };
        }
    }

    public Task<ApiResponse<ModuleData>> GetSingleAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<ModuleData>> AddAsync(ModuleData dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<ModuleData>> UpdateAsync(Guid id, ModuleData dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<ModuleData>> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    #endregion
}