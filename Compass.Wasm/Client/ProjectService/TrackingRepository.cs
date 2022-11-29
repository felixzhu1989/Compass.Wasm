using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.ProjectService;

public class TrackingRepository:ITrackingRepository
{
    public List<TrackingResponse> Trackings { get; set; }
    public string Message { get; set; }
    public Task SearchTrackings(string searchText)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetProjectSearchSuggestions(string searchText)
    {
        throw new NotImplementedException();
    }
}