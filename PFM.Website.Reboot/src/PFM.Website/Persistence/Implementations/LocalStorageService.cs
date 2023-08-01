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
            try
            {
                var path = p.FileName;
                if (File.Exists(path) && path.Contains(p.Location))
                {
                    File.Delete(path);
                }
                return Task.CompletedTask;
            }
            catch (IOException ex)
            {
                _logger.Error(ex, "IO exception");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception");
                throw;
            }
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
            catch (IOException ex)
            {
                _logger.Error(ex, "IO exception");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception");
                throw;
            }
        }
    }
}

