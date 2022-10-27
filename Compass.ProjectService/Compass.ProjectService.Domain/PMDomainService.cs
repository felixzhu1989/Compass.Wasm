namespace Compass.ProjectService.Domain;
/// <summary>
/// 项目管理内部逻辑，面向接口编程，不考虑实现
/// </summary>
public class PMDomainService
{
    private readonly IPMRepository _repository;
    public PMDomainService(IPMRepository repository)
    {
        _repository = repository;
    }


}