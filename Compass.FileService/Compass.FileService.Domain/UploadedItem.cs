using Zack.DomainCommons.Models;

namespace Compass.FileService.Domain
{
    //继承自BaseEntity,已经设定Guid类型的主键Id，以及其他领域事件DomainEvent相关的操作
    /// <summary>
    /// 文件上传项，每个上传的文件都对应一个上传项
    /// </summary>
    public record UploadedItem : BaseEntity, IHasCreationTime
    {
        public DateTime CreationTime { get; private set; }//继承自IHasCreationTime，上传时间
        /// <summary>
        /// 文件大小（单位为字节）
        /// </summary>
        public long FileSizeInBytes { get; private set; }
        /// <summary>
        /// 用户上传的原始文件名，没有包含路径
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 文件的SHA256散列值（非常重要的属性）
        /// </summary>
        public string FileSHA256Hash { get; private set; }
        /// <summary>
        /// 备份文件路径
        /// </summary>
        public Uri BackupUrl { get; private set; }
        /// <summary>
        /// 云存储路径,上传的文件的供外部访问者访问的路径（演示时存在服务器上一个路径）
        /// </summary>
        public Uri RemoteUrl { get; private set; }

        /// <summary>
        /// 标记是否为旧文件，不存储到数据库
        /// </summary>
        public bool IsOldFile { get; set; }//特征5，不映射到数据库

        //创建类的对象实例(其实就是初始化)，因为都是只读属性
        public static UploadedItem Create(Guid id, long fileSizeInBytes, string fileName, string fileSHA256Hash,
            Uri backupUrl, Uri remoteUrl)
        {
            UploadedItem item = new UploadedItem()
            {
                Id=id,
                CreationTime = DateTime.Now,
                FileName=fileName,
                FileSHA256Hash = fileSHA256Hash,
                FileSizeInBytes=fileSizeInBytes,
                BackupUrl = backupUrl,
                RemoteUrl=remoteUrl
            };
            return item;
        }
    }
}