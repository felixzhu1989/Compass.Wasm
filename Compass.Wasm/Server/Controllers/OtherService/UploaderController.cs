using Compass.Wasm.Shared.FileService;
using Compass.Wasm.Server.FileService;

namespace Compass.Wasm.Server.Controllers.OtherService
{
    //FileService
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin,pm")]//权限
    [UnitOfWork(typeof(FileDbContext))]
    public class UploaderController : ControllerBase
    {
        private readonly FileDomainService _domainService;
        private readonly FileDbContext _dbContext;
        private readonly IFileRepository _repository;
        public UploaderController(FileDomainService domainService, FileDbContext dbContext, IFileRepository repository)
        {
            _domainService = domainService;
            _dbContext = dbContext;
            _repository = repository;
        }
        [HttpGet("FileExists")]
        public async Task<FileExistsResponse> FileExists(long fileSize, string sha256Hash)
        {
            var item = await _repository.FindFileAsync(fileSize, sha256Hash);
            if (item == null) return new FileExistsResponse(false, null);
            else return new FileExistsResponse(true, item.RemoteUrl);
        }

        //todo: 做好校验，参考OSS的接口，防止被滥用
        //todo：应该由应用服务器向fileserver申请一个上传码（可以指定申请的个数，这个接口只能供应用服务器调用），
        //页面直传只能使用上传码上传一个文件，防止接口被恶意利用。应用服务器要控制发放上传码的频率。
        //todo：再提供一个非页面直传的接口，供服务器用
        [HttpPost("ApiUpload")]
        [RequestSizeLimit(100_000_000)]
        public async Task<ActionResult<Uri>> ApiUpload([FromForm] UploadRequest request,
            CancellationToken cancellationToken = default)
        {
            var file = request.File;
            string fileName = file!.FileName;
            await using Stream stream = file.OpenReadStream();
            var upItem = await _domainService.UploadAsync(stream, fileName, cancellationToken);
            _dbContext.Add(upItem);
            return upItem.RemoteUrl;
        }

        //用于Compass.Wasm.Client
        [HttpPost]
        public async Task<ActionResult<UploadResponse>> Upload(IEnumerable<IFormFile> files, CancellationToken cancellationToken = default)
        {
            var file = files.First();//针对单个文件
            string fileName = file.FileName;
            await using Stream stream = file.OpenReadStream();
            var upItem = await _domainService.UploadAsync(stream, fileName, cancellationToken);
            if (!upItem.IsOldFile) _dbContext.Add(upItem);
            return new UploadResponse(upItem.RemoteUrl);
        }
    }
}
