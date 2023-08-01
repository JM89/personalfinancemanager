using PFM.Website.Persistence.Models;

namespace PFM.Website.Persistence
{
	public interface IObjectStorageService
	{
        Task<string> UploadFileAsync(ObjectStorageParams p, Stream file);
        Task<TransferFile?> DownloadFileAsync(ObjectStorageParams p);
        Task DeleteFileAsync(ObjectStorageParams p);
    }
}