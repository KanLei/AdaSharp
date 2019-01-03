using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdaSharp
{
    public class ApiClient
    {
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
        private readonly HttpClient client;

        public ApiClient(HttpClient client)
        {
            this.client = client;
        }

        public ApiClient(HttpMessageHandler handler) : this(new HttpClient(handler)) { }

        public ApiClient() : this(new HttpClient()) { }

        #region Public Methods

        public async Task<TResult> GetAsync<TResult>(string url)
        {
            return await SendRequstAsync<TResult>(() => GetImplAsync(url)).ConfigureAwait(false);
        }

        public async Task<TResult> PostAsync<TResult>(string url, object data)
        {
            return await SendRequstAsync<TResult>(() => PostImplAsync(url, data)).ConfigureAwait(false);
        }

        public async Task<TResult> PutAsync<TResult>(string url, object data)
        {
            return await SendRequstAsync<TResult>(() => PutImplAsync(url, data)).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string url)
        {
            await DeleteImplAsync(url).ConfigureAwait(false);
        }

        #endregion

        private async Task<TResult> SendRequstAsync<TResult>(Func<Task<(bool, string)>> reqeustHandler)
        {
            (bool success, string message) = await reqeustHandler().ConfigureAwait(false);

            if (success)
            {
                return JsonConvert.DeserializeObject<TResult>(message, jsonSettings);
            }

            throw new Exception($"Send request fail {message}");
        }

        private async Task<(bool, string)> GetImplAsync(string url)
        {
            try
            {
                var responseMessage = await client.GetAsync(url).ConfigureAwait(false);
                string message = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                return (responseMessage.IsSuccessStatusCode, message);
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"Timeout POST request {url}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error POST request {url}", ex);
            }
        }

        private async Task<(bool, string)> PostImplAsync(string url, object data)
        {
            try
            {
                var content = JsonConvert.SerializeObject(data, jsonSettings);
                var strContent = new StringContent(content, Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync(url, strContent).ConfigureAwait(false);
                string message = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                return (responseMessage.IsSuccessStatusCode, message);
            }
            catch (JsonSerializationException ex)
            {
                throw new ArgumentException($"Serialize {data.GetType()} to json fail", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"Timeout POST request {url}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error POST request {url}", ex);
            }
        }

        private async Task<(bool, string)> PutImplAsync(string url, object data)
        {
            try
            {
                var content = JsonConvert.SerializeObject(data, jsonSettings);
                var strContent = new StringContent(content, Encoding.UTF8, "application/json");
                var responseMessage = await client.PutAsync(url, strContent).ConfigureAwait(false);
                string message = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                return (responseMessage.IsSuccessStatusCode, message);
            }
            catch (JsonSerializationException ex)
            {
                throw new ArgumentException($"Serialize {data.GetType()} to json fail", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"Timeout PUT request {url}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error PUT request {url}", ex);
            }
        }

        private async Task<(bool, string)> DeleteImplAsync(string url)
        {
            try
            {
                var responseMessage = await client.DeleteAsync(url).ConfigureAwait(false);
                return (responseMessage.IsSuccessStatusCode, "");
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"Timeout DELETE request {url}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error DELETE request {url}", ex);
            }
        }
    }
}

