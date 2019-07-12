using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RestAPIClient
{
    public class RestClient
    {
        private Uri _endPointUrl;
        private static HttpClient _client = new HttpClient();
        private static JavaScriptSerializer _jsonSerializer = new JavaScriptSerializer();

        protected RestClient(string endPointUrl)
        {
            _endPointUrl = new Uri(endPointUrl);
        }

        private static StringContent GetJSONStringContent(object o)
        {
            return new StringContent(_jsonSerializer.Serialize(o), Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Gets a typed object from the specifed resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<T> GetObjectAsync<T>(string resource, int id)
        {
            Uri requestUri = new Uri(_endPointUrl, string.Format("{0}/{1}", resource, id));

            HttpResponseMessage response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                T o = _jsonSerializer.Deserialize<T>(json);

                return o;
            }

            return default(T);
        }

        /// <summary>
        /// Gets an array of typed objects from the specified resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The endpoint resource, note that if this parameter starts with a / it will remove all path elements of the endpoint url this class was initialized with</param>
        /// <returns></returns>
        protected async Task<T[]> GetObjectsAsync<T>(string resource)
        {
            return await GetObjectsAsync<T>(resource, null, null);
        }

        /// <summary>
        /// Gets an array of typed objects from the specified resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The endpoint resource, note that if this parameter starts with a / it will remove all path elements of the endpoint url this class was initialized with</param>
        /// <returns></returns>
        protected async Task<T[]> GetObjectsAsync<T>(string resource, string idName, int? idValue)
        {
            Uri requestUri = new Uri(_endPointUrl, resource);

            if (!string.IsNullOrEmpty(idName) && idValue != null)
            {
                requestUri = new Uri(requestUri, string.Format("?{0}={1}", idName, idValue.Value));
            }

            HttpResponseMessage response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                T[] objects = _jsonSerializer.Deserialize<T[]>(json);

                return objects;
            }

            return null;
        }

        /// <summary>
        /// Creates an object using the specified resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        protected async Task<T> CreateObjectAsync<T>(string resource, T o)
        {
            Uri requestUri = new Uri(_endPointUrl, resource);

            HttpResponseMessage response = await _client.PostAsync(requestUri, GetJSONStringContent(o));

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                o = _jsonSerializer.Deserialize<T>(json);

                return o;
            }

            return default(T);
        }

        /// <summary>
        /// Updates an object using the specified resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <param name="id"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        protected async Task<T> UpdateObjectAsync<T>(string resource, int id, T o)
        {
            Uri requestUri = new Uri(_endPointUrl, string.Format("{0}/{1}", resource, id));

            HttpResponseMessage response = await _client.PatchAsync(requestUri, GetJSONStringContent(o));

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                o = _jsonSerializer.Deserialize<T>(json);

                return o;
            }

            return default(T);
        }

        /// <summary>
        /// Deletes an object specified by id
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<bool> DeleteObject(string resource, int id)
        {
            Uri requestUri = new Uri(_endPointUrl, string.Format("{0}/{1}", resource, id));

            HttpResponseMessage response = await _client.DeleteAsync(requestUri);

            return response.IsSuccessStatusCode;
        }
    }
}