using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            client.DefaultRequestHeaders.Add("X-Rundeck-Auth-Token", authToken);
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Rundeck client");
            baseUrl = scheme + host + "/api/33";
        }

        private string getUrl(string url)
        {
            return $"{baseUrl}/{url}";
        }

        public async Task<List<Project>> GetProjects()
        {
            var response =  await client.GetStringAsync(getUrl("projects"));
            var projects = JsonConvert.DeserializeObject<List<Project>>(response);
            return projects;
        }

        public async Task<List<Job>> GetJobs(string projectUrl)
        {
            var response =  await client.GetStringAsync(projectUrl + "/jobs");
            return JsonConvert.DeserializeObject<List<Job>>(response);
        }
        
        public async Task<Execution> RunJob(string jobId, Dictionary<string, string> options = null)
        {
            var content = new FormUrlEncodedContent(options ?? new Dictionary<string, string>());
            var response = await client.PostAsync(getUrl($"job/{jobId}/executions"), content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jobResponse = JsonConvert.DeserializeObject<Execution>(responseString);
            return jobResponse;
        }
        
        public async Task<Execution> GetExecution(string executionId)
        {
            var response = await client.GetStringAsync(getUrl($"execution/{executionId}"));
            var jobResponse = JsonConvert.DeserializeObject<Execution>(response);
            return jobResponse;
        }
        
        public async Task<string> GetExecutionOutput(string executionId)
        {
            return await client.GetStringAsync(getUrl($"execution/{executionId}/output?format=text"));
        }
        
    }
    
}