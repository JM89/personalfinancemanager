﻿using Newtonsoft.Json;
using PersonalFinanceManager.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PersonalFinanceManager.Services.HttpClientWrapper
{
    public class HttpClientExtended : IHttpClientExtended
    {
        private readonly string _apiBaseUrl;
        private readonly Serilog.ILogger _logger;
        private readonly HttpClient _httpClient;

        public HttpClientExtended(Serilog.ILogger logger, HttpClient httpClient)
        {
            _apiBaseUrl = ConfigurationManager.AppSettings["PfmApiUrl"];
            _logger = logger;
            this._httpClient = httpClient;
        }

        public async Task<IList<TResult>> GetList<TResult>(string url, HttpClientRequestOptions opts = null)
        {
            if (opts == null)
            {
                opts = new HttpClientRequestOptions();
            }

            IList<TResult> result = null;

            try
            {
                if (opts.AuthenticationTokenRequired)
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", GetAccessToken());
                }

                var endpoint = _apiBaseUrl + url;
                var httpResponse = await _httpClient.GetAsync(endpoint);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<IList<TResult>>(content).ToList();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception occurred while calling GetList method");
                throw;
            }
            return result;
        }

        public async Task<TResult> GetSingle<TResult>(string url, HttpClientRequestOptions opts = null)
        {
            if (opts == null)
            {
                opts = new HttpClientRequestOptions();
            }
            TResult result = default(TResult);
           
            try
            {
                if (opts.AuthenticationTokenRequired)
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", GetAccessToken());
                }

                var endpoint = _apiBaseUrl + url;
                var httpResponse = await _httpClient.GetAsync(endpoint);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<TResult>(content);
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception occurred while calling GetSingle method");
                throw;
            }
            return result;
        }

        public async Task<IList<TResult>> GetListBySearchParameters<TResult, TParams>(string url, TParams searchParameters, HttpClientRequestOptions opts = null)
        {
            if (opts == null)
            {
                opts = new HttpClientRequestOptions();
            }

            IList<TResult> result = null;

            try 
            {
                if (opts.AuthenticationTokenRequired)
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", GetAccessToken());
                }
                var endpoint = _apiBaseUrl + url;
                var json = JsonConvert.SerializeObject(searchParameters);
                var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync(endpoint, requestBody);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<IList<TResult>>(content).ToList();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception occurred while calling GetListBySearchParameters method");
                throw;
            }
            return result;
        }

        public async Task<bool> Post<TObject>(string url, TObject obj, HttpClientRequestOptions opts = null)
        {
            return await Post<TObject, object>(url, obj, opts) != null;
        }

        public async Task<TResult> Post<TObject, TResult>(string url, TObject obj, HttpClientRequestOptions opts = null)
        {
            if (opts == null)
            {
                opts = new HttpClientRequestOptions();
            }
            TResult result = default(TResult);

            try
            {
                if (opts.AuthenticationTokenRequired)
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", GetAccessToken());
                }

                var endpoint = _apiBaseUrl + url;
                var json = JsonConvert.SerializeObject(obj);
                var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync(endpoint, requestBody);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<TResult>(content);
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception occurred while calling Post method");
                throw;
            }

            return result;
        }

        public async Task<bool> Post(string url, HttpClientRequestOptions opts = null)
        {
            if (opts == null)
            {
                opts = new HttpClientRequestOptions();
            }

            try
            {
                if (opts.AuthenticationTokenRequired)
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", GetAccessToken());
                }
                var endpoint = _apiBaseUrl + url;

                var httpResponse = await _httpClient.PostAsync(endpoint, null);
                if (httpResponse.IsSuccessStatusCode)
                {
                    await httpResponse.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception occurred while calling Post method");
                throw;
            }
        }

        public async Task<bool> Put<TObject>(string url, TObject obj, HttpClientRequestOptions opts = null)
        {
            if (opts == null)
            {
                opts = new HttpClientRequestOptions();
            }

            try
            {
                if (opts.AuthenticationTokenRequired)
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", GetAccessToken());
                }
                var endpoint = _apiBaseUrl + url;

                var json = JsonConvert.SerializeObject(obj);
                var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PutAsync(endpoint, requestBody);
                if (httpResponse.IsSuccessStatusCode)
                {
                    await httpResponse.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception occurred while calling Put method");
                throw;
            }
        }

        public async Task<bool> Delete(string url, HttpClientRequestOptions opts = null)
        {
            if (opts == null)
            {
                opts = new HttpClientRequestOptions();
            }

            try
            {
                if (opts.AuthenticationTokenRequired)
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", GetAccessToken());
                }

                var endpoint = _apiBaseUrl + url;
                var httpResponse = await _httpClient.DeleteAsync(endpoint);
                if (httpResponse.IsSuccessStatusCode)
                {
                    await httpResponse.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new ApiException(url, httpResponse.StatusCode.ToString());
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception occurred while calling Delete method");
                throw;
            }
        }

        private string GetAccessToken()
        {
            var identity = (ClaimsPrincipal)HttpContext.Current.User;
            var token = identity.Claims.SingleOrDefault(x => x.Type == "AccessToken");
            if (token?.Value == null)
            {
                throw new Exception("Invalid Token");
            }
            return token.Value;
        }
    }
}