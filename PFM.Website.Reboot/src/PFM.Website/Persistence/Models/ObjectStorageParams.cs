using System;
namespace PFM.Website.Persistence.Models
{
	public class ObjectStorageParams
	{
        public string Location { get; }

        public string FileName { get; }

        public string ContentType { get; }

        public ObjectStorageParams(string contentType, string bucket, string filename)
        {
            ContentType = contentType;
            Location = bucket;
            FileName = filename;
        }
    }
}