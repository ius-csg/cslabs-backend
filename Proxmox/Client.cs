using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rundeck
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
            client.DefaultRequestHeaders.Add("X-Proxmox-Auth-Token", authToken);
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Proxmox client");
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
        
        public async Task<Execution> RunJob(string jobId, ExecutionParams options = null)
        {
            var strContent = JsonConvert.SerializeObject(options ?? new ExecutionParams());
            var content = new StringContent(strContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(getUrl($"job/{jobId}/executions"), content);
            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new RundeckException(JsonConvert.DeserializeObject<ErrorResponse>(responseString));
            
            var jobResponse = JsonConvert.DeserializeObject<Execution>(responseString);
            return jobResponse;
        }

        public async Task<string> RunJobAndGetOutput(string jobId, ExecutionParams options = null)
        {
            var execution = await RunJob(jobId, options);
            while (execution.Status == "running")
            {
                execution = await GetExecution(execution.Id);
            }
            
            String output =  await GetExecutionOutput(execution.Id);
            while (output.Length == 0)
            {
                output =  await GetExecutionOutput(execution.Id);
            }

            return output;
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