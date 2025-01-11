using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace PFM.TNP.Api.Contracts.Shared
{
    public class ApiResponse
    {
        public object Data { get; set; }

        public IDictionary<string, List<string>> Errors { get; set; }

        public ApiResponse() { }

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
