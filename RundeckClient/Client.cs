using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RundeckClient
{
    public class Client
    {
        private string host;
        private readonly HttpClient client = new HttpClient();
        private readonly string baseUrl;
        private readonly string authToken;
        public Client(string scheme, string host, string authToken)
        {
            this.host = host;
            this.authToken = authToken;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Rundeck client");
            baseUrl = scheme + host + "/api/14";
        }

        private string getUrl(string url)
        {
            return $"{baseUrl}/{url}?authtoken={authToken}";
        }

        public async Task<string> GetProjects()
        {
            var response =  await client.GetStringAsync(getUrl("projects"));
            return response;
        }
        
        
    }
    
}