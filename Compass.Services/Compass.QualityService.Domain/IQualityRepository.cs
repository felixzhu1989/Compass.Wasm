using Compass.QualityService.Domain.Entities;

namespace Compass.QualityService.Domain;

public interface IQualityRepository
{
    Task<FinalInspectionCheckItemType?> GetFinalInspectionCheckItemTypeByIdAsync(Guid id);
    Task<IQueryable<FinalInspectionCheckItemType>> GetFinalInspectionCheckItemTypesAsync();
    Task<int> GetMaxSeqOfFinalInspectionCheckItemTypesAsync();//获取最大序号
}