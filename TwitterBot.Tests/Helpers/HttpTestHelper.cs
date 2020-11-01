using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.IO;
using System.Text;

namespace TwitterBot.Tests.Helpers
{
    public class HttpTestHelper
    {
        public static HttpRequest CreateHttpRequest(
            string url, 
            string method, 
            IHeaderDictionary headers = null, 
            string body = null)
        {
            var uri = new Uri(url);
            var request = new DefaultHttpContext().Request;
            
            var requestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();
            requestFeature.Method = method;
            requestFeature.Scheme = uri.Scheme;
            requestFeature.Path = uri.GetComponents(UriComponents.KeepDelimiter | UriComponents.Path, UriFormat.Unescaped);
            requestFeature.PathBase = string.Empty;
            requestFeature.QueryString = uri.GetComponents(UriComponents.KeepDelimiter | UriComponents.Query, UriFormat.Unescaped);
            headers = headers ?? new HeaderDictionary();
            
            if (!string.IsNullOrEmpty(uri.Host))
            {
                headers.Add("Host", uri.Host);
            }

            if (body != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(body);
                
                requestFeature.Body = new MemoryStream(bytes);
                request.ContentLength = request.Body.Length;
                
                headers.Add("Content-Length", request.Body.Length.ToString());
            }
            requestFeature.Headers = headers;
            
            return request;
        }
    }
}
