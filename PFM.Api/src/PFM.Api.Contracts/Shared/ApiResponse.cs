using System.Collections.Generic;

namespace PFM.Api.Contracts.Shared
{
    public class ApiResponse
    {
        public object Data { get; }

        public IDictionary<string, List<string>> Errors { get; }

        public ApiResponse(object data)
        {
            this.Data = data;
        }

        public ApiResponse(string error)
        {
            this.Errors = new Dictionary<string,  List<string>>()
            {
                { "default", new List<string> { error} }
            };
        }

        public ApiResponse(IDictionary<string, List<string>> errors)
        {
            this.Errors = errors;
        }
    }
}
