using PFM.Services.Persistence.Models;

namespace PFM.Services.Persistence
{
	public interface IObjectStorageService
	{
        Task<string> UploadFileAsync(ObjectStorageParams p, Stream file);
        Task<TransferFile?> DownloadFileAsync(ObjectStorageParams p);
        Task DeleteFileAsync(ObjectStorageParams p);
    }
}