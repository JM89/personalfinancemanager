using PFM.Website.Persistence.Models;

namespace PFM.Website.Persistence.Implementations
{
	public class LocalStorageService : IObjectStorageService
	{
        private readonly Serilog.ILogger _logger;

        public LocalStorageService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public Task DeleteFileAsync(ObjectStorageParams p)
        {
            throw new NotImplementedException();
        }

        public Task<TransferFile> DownloadFileAsync(ObjectStorageParams p)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadFileAsync(ObjectStorageParams p, Stream file)
        {
            try
            {
                var path = Path.Combine(p.Location, p.FileName);
                using (var outputFileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(outputFileStream);
                }
                return await Task.FromResult(path);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Unhandled exception");
                throw;
            }
        }
    }
}

