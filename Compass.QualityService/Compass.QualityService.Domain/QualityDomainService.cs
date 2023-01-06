using Compass.QualityService.Domain.Entities;

namespace Compass.QualityService.Domain;

public class QualityDomainService
{
    private readonly IQualityRepository _repository;
    public QualityDomainService(IQualityRepository repository)
    {
        _repository = repository;
    }

    public async Task<FinalInspectionCheckItemType> AddFinalInspectionCheckItemTypeAsync(string name)
    {
        int maxSeq = await _repository.GetMaxSeqOfFinalInspectionCheckItemTypesAsync();
        return new FinalInspectionCheckItemType(Guid.NewGuid(), maxSeq+1, name);
    }
    public async Task SortProductsAsync(Guid[] sortedCheckItemIds)
    {
        var checkItemTypes = await _repository.GetFinalInspectionCheckItemTypesAsync();
        var idsInDb = checkItemTypes.Select(x => x.Id);
        if (!idsInDb.SequenceIgnoredEqual(sortedCheckItemIds))
        {
            throw new Exception($"提交的待排序Id中必须是FinalInspectionCheckItemType所有的Id");
        }
        int seqNum = 1;
        foreach (var checkItemTypeId in sortedCheckItemIds)
        {
            var checkItemType = await _repository.GetFinalInspectionCheckItemTypeByIdAsync(checkItemTypeId);
            if (checkItemType == null) throw new Exception($"productId={checkItemTypeId}不存在");
            checkItemType.ChangeSequenceNumber(seqNum);
            seqNum++;
        }
    }
}