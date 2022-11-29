using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.ProjectService;

public interface ITrackingRepository
{
    List<TrackingResponse> Trackings { get; set; }
    string Message { get; set; }
    Task SearchTrackings(string searchText);
    Task<List<string>> GetProjectSearchSuggestions(string searchText);
}