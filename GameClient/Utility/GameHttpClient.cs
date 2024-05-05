using GameClient.Utility.Interface;
using GameLibrary.Core.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameClient.Utility
{
    public class GameHttpClient : IGameHttpClient
    {
        private readonly HttpClient httpClient;

        public GameHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:10123/Api/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient = client;
        }

        async Task<UserInfo?> IGameHttpClient.GetUserInfoAsync(string userCode, string password)
        {
            try
            {
                var res = await httpClient.GetAsync("/Api/LoginIn?userCode=" + userCode + "&password=" + password);
                if (res.IsSuccessStatusCode)
                {
                    string str = await res.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserInfo>(str);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
