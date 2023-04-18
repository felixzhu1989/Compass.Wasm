namespace Compass.ProjectService.Domain;
/// <summary>
/// 项目管理内部逻辑，面向接口编程，不考虑实现
/// </summary>
public class ProjectDomainService
{
    private readonly IProjectRepository _repository;
    public ProjectDomainService(IProjectRepository repository)
    {
        _repository = repository;
    }
    




}