namespace PFM.Services.Persistence.Models
{
	public class ObjectStorageParams
	{
        public string Location { get; }

        public string FileName { get; }

        public ObjectStorageParams(string bucket, string filename)
        {
            Location = bucket;
            FileName = filename;
        }
    }
}