using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using RomeApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RomeApp.Repositories
{
    public class GraphRepository : IDisposable
    {
        private static string GraphAPIBetaEndpoint { get; } = "https://graph.microsoft.com/beta/{0}";
        public string AccessToken { get; }
        private GraphServiceClient GraphServiceClient { get; }
        private HttpClient HttpClient { get; }
        public GraphRepository(string accessToken)
        {
            AccessToken = accessToken;
            GraphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(rm =>
            {
                rm.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                return Task.CompletedTask;
            }));
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        }

        public Task<User> GetUserAsync() => GraphServiceClient.Me.Request().GetAsync();

        public async Task<IEnumerable<DeviceInfo>> GetDevicesAsync()
        {
            var response = await HttpClient.GetAsync(string.Format(GraphAPIBetaEndpoint, "me/devices"));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsAsync<JObject>();
            return json.GetValue("value").ToObject<IEnumerable<DeviceInfo>>();
        }

        public async Task AddLaunchAppRequestAsync(string deviceId, string url)
        {
            var urlToken = string.Format("me/devices/{0}/commands", deviceId);
            var response = await HttpClient.PostAsJsonAsync(string.Format(GraphAPIBetaEndpoint, urlToken), new CommandRequest
            {
                Payload = new Payload
                {
                    Uri = url,
                },
            });

            Trace.WriteLine($"deviceId: {deviceId}, url: {url}");
            Trace.WriteLine($"response.IsSuccessStatusCode: {response.IsSuccessStatusCode}");
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    HttpClient.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
