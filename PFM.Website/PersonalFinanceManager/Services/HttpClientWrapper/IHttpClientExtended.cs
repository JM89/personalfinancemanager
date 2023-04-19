using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.HttpClientWrapper
{
    public interface IHttpClientExtended
    {
        Task<IList<TResult>> GetList<TResult>(string url, HttpClientRequestOptions opts = null);

        Task<TResult> GetSingle<TResult>(string url, HttpClientRequestOptions opts = null);

        Task<IList<TResult>> GetListBySearchParameters<TResult, TParams>(string url, TParams searchParameters, HttpClientRequestOptions opts = null);

        Task<bool> Post<TObject>(string url, TObject obj, HttpClientRequestOptions opts = null);

        Task<TResult> Post<TObject, TResult>(string url, TObject obj, HttpClientRequestOptions opts = null);

        Task<bool> Post(string url, HttpClientRequestOptions opts = null);

        Task<bool> Put<TObject>(string url, TObject obj, HttpClientRequestOptions opts = null);

        Task<bool> Delete(string url, HttpClientRequestOptions opts = null);
    }
}
