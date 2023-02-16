using Compass.Wasm.Shared.TodoService;

namespace Compass.Wpf.Service;

public class MemoService : BaseService<MemoDto>, IMemoService
{
    public MemoService(HttpRestClient client) : base(client, "Memo")
    {
    }
}