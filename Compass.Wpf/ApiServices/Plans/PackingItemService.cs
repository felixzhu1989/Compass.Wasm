using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Plans;

public interface IPackingItemService : IBaseService<PackingItemDto>
{

}

public class PackingItemService:BaseService<PackingItemDto>,IPackingItemService
{
    public PackingItemService(HttpRestClient client) : base(client, "PackingItem")
    {
    }
}