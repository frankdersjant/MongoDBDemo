using System;
using System.Net.Http;
using System.Net.Http.Headers;
using WebAppClient.Constants;

namespace WebAppClient.Helpers
{
    public static class MVCClientHttpClient
    {
        public static HttpClient GetClient(string requestedVersion = null)
        {

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(MVCClientHttpClientConstants.MVCClientHttpClientAPI);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }

}
