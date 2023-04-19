using System.Collections.Generic;

namespace PersonalFinanceManager.Services.HttpClientWrapper
{
    public interface IHttpClientExtended
    {
        IList<TResult> GetList<TResult>(string url, HttpClientRequestOptions opts = null);

        TResult GetSingle<TResult>(string url, HttpClientRequestOptions opts = null);

        IList<TResult> GetListBySearchParameters<TResult, TParams>(string url, TParams searchParameters, HttpClientRequestOptions opts = null);

        void Post<TObject>(string url, TObject obj, HttpClientRequestOptions opts = null);

        TResult Post<TObject, TResult>(string url, TObject obj, HttpClientRequestOptions opts = null);

        void Post(string url, HttpClientRequestOptions opts = null);

        void Put<TObject>(string url, TObject obj, HttpClientRequestOptions opts = null);

        void Delete(string url, HttpClientRequestOptions opts = null);
    }
}
