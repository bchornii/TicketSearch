using System;
using System.Net.Http;

namespace RailwayTicketSearch.Infrastructure
{
    public class HttpRequestBuilder
    {        
        private string _requestUri;
        private HttpContent _content;
        private HttpMethod _method;

        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            _requestUri = requestUri;            
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            _content = content;
            return this;
        }

        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }

        public HttpRequestMessage GetRequestMessage()
        {
            var request = new HttpRequestMessage
            {
                Method = _method,
                RequestUri = new Uri(_requestUri),
                Content = _content
            };            
            request.Headers.Add("Accept", "text/xml");
            return request;
        }
    }
}