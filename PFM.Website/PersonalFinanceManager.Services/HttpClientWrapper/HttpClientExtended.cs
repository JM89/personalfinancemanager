using Newtonsoft.Json;
using PersonalFinanceManager.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.HttpClientWrapper
{
    public class HttpClientExtended : HttpClient
    {
        private readonly string _apiBaseUrl;

        public HttpClientExtended()
        {
            _apiBaseUrl = ConfigurationManager.AppSettings["PfmApiUrl"];
        }

        public IList<TResult> GetList<TResult>(string url)
        {
            IList<TResult> result = null;
            using (var httpClient = new HttpClient())
            {
                var endpoint = _apiBaseUrl+ url;
                var call = httpClient.GetAsync(endpoint);
                call.Wait();
                var httpResponse = call.Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = httpResponse.Content.ReadAsStringAsync();
                    content.Wait();
                    result = JsonConvert.DeserializeObject<IList<TResult>>(content.Result).ToList();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
            return result;
        }

        public TResult GetSingle<TResult>(string url)
        {
            TResult result = default(TResult);
            using (var httpClient = new HttpClient())
            {
                var endpoint = _apiBaseUrl + url;
                var call = httpClient.GetAsync(endpoint);
                call.Wait();
                var httpResponse = call.Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = httpResponse.Content.ReadAsStringAsync();
                    content.Wait();
                    result = JsonConvert.DeserializeObject<TResult>(content.Result);
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
            return result;
        }

        public IList<TResult> GetListBySearchParameters<TResult, TParams>(string url, TParams searchParameters)
        {
            IList<TResult> result = null;
            using (var httpClient = new HttpClient())
            {
                var endpoint = _apiBaseUrl + url;
                var json = JsonConvert.SerializeObject(searchParameters);
                var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
                var call = httpClient.PostAsync(endpoint, requestBody);
                call.Wait();
                var httpResponse = call.Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = httpResponse.Content.ReadAsStringAsync();
                    content.Wait();
                    result = JsonConvert.DeserializeObject<IList<TResult>>(content.Result).ToList();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
            return result;
        }

        public void Post<TObject>(string url, TObject obj)
        {
            Post<TObject, object>(url, obj);
        }

        public TResult Post<TObject, TResult>(string url, TObject obj)
        {
            TResult result = default(TResult);
            using (var httpClient = new HttpClient())
            {
                var endpoint = _apiBaseUrl + url;
                var json = JsonConvert.SerializeObject(obj);
                var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
                var call = httpClient.PostAsync(endpoint, requestBody);
                call.Wait();
                var httpResponse = call.Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = httpResponse.Content.ReadAsStringAsync();
                    content.Wait();
                    result = JsonConvert.DeserializeObject<TResult>(content.Result);
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
            return result;
        }

        public void Post(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var endpoint = _apiBaseUrl + url;

                var call = httpClient.PostAsync(endpoint, null);
                call.Wait();
                var httpResponse = call.Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = httpResponse.Content.ReadAsStringAsync();
                    content.Wait();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
        }

        public void Put<TObject>(string url, TObject obj)
        {
            using (var httpClient = new HttpClient())
            {
                var endpoint = _apiBaseUrl + url;

                var json = JsonConvert.SerializeObject(obj);
                var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
                var call = httpClient.PutAsync(endpoint, requestBody);
                call.Wait();
                var httpResponse = call.Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = httpResponse.Content.ReadAsStringAsync();
                    content.Wait();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
        }

        public void Delete(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var endpoint = _apiBaseUrl + url;
                var call = httpClient.DeleteAsync(endpoint);
                call.Wait();
                var httpResponse = call.Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = httpResponse.Content.ReadAsStringAsync();
                    content.Wait();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
        }
    }
}
