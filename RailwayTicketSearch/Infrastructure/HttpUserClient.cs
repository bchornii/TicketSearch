using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RailwayTicketSearch.Infrastructure
{
    public class HttpUserClient
    {
        public async Task<HttpResponseMessage> FormUrlEncodedPost(string requestUri, 
            IEnumerable<KeyValuePair<string, string>> content)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Post)
                .AddRequestUri(requestUri)
                .AddContent(new FormUrlEncodedContent(content));
            var requestMessage = builder.GetRequestMessage();

            using (var client = new HttpClient())
            {
                return await client.SendAsync(requestMessage);                
            }
        }

        public async Task<string> FormUrlEncodedPostAsString(string requestUri,
            IEnumerable<KeyValuePair<string, string>> content)
        {
            var response = await FormUrlEncodedPost(requestUri, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}