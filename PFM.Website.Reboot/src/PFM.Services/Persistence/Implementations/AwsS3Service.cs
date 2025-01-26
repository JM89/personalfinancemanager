using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using PFM.Services.Persistence.Models;

namespace PFM.Services.Persistence.Implementations
{
	public class AwsS3Service : IObjectStorageService
    {
        private readonly IAmazonS3 _client;
        private readonly Serilog.ILogger _logger;

        public AwsS3Service(Serilog.ILogger logger, IAmazonS3 client)
        {
            _client = client;
            _logger = logger;
        }

        public async Task DeleteFileAsync(ObjectStorageParams p)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = p.Location,
                    Key = p.FileName
                };

                var result = await _client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception ex)
            {
                _logger.Error(ex, "S3 exception");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception");
                throw;
            }
        }

        public async Task<TransferFile?> DownloadFileAsync(ObjectStorageParams p)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = p.Location,
                    Key = p.FileName
                };

                using (var objectResponse = await _client.GetObjectAsync(request))
                {
                    if (objectResponse.HttpStatusCode == HttpStatusCode.NotFound)
                    {
                        throw new Exception("Could not find file.");
                    }

                    using (var responseStream = objectResponse.ResponseStream)
                    using (var reader = new StreamReader(responseStream))
                    {
                        var result = new MemoryStream();
                        responseStream.CopyTo(result);
                        return new TransferFile
                        {
                            Name = p.FileName,
                            Stream = result
                        };
                    }
                }
            }
            catch (AmazonS3Exception ex)
            {
                _logger.Error(ex, "S3 exception");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception");
                throw;
            }
        }

        public async Task<string> UploadFileAsync(ObjectStorageParams p, Stream file)
        {
            try
            {
                var objs = await _client.ListObjectsV2Async(new ListObjectsV2Request() {
                    BucketName= p.Location
                });

                var request = new PutObjectRequest()
                {
                    BucketName = p.Location,
                    Key = p.FileName,
                    InputStream = file
                };

                await _client.PutObjectAsync(request);

                return p.FileName;
            }
            catch (AmazonS3Exception ex)
            {
                _logger.Error(ex, "S3 exception");
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

