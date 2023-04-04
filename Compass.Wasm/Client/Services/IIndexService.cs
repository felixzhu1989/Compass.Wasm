using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.Services;

public interface IIndexService
{
    event Action TrackingsChanged;//当Trackings发生改变时需要及时更新
    List<IndexModel> Trackings { get; set; }
    string Message { get; set; }
    int CurrentPage { get; set; }
    int PageCount { get; set; }
    string LastSearchText { get; set; }
    Task GetTrackingsAsync(int page);
    Task SearchTrackings(string searchText, int page);
    //Task<List<string>> GetTrackingSearchSuggestions(string searchText);
}