namespace HireMe.Payments
{
using HireMe.Payments.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

   /* public interface IPaypalClient
    {
        Task<bool> CaptureOrder(AccessToken accesstoken, string token);
        Task<OrderResult> CreateOrder(Order order, AccessToken token);
        Task<AccessToken> GetToken(string clientID, string secretID);
    }*/

    public class PaypalClient //: IPaypalClient
    {
        private readonly HttpClient _client;

        public PaypalClient(HttpClient client)
        {
            _client = client;
        }
        public async Task<AccessToken> GetToken(string clientID, string secretID)
        {
            var authToken = Encoding.ASCII.GetBytes($"{clientID}:{secretID}");

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri("https://api-m.sandbox.paypal.com/v1/oauth2/token");
            request.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(authToken)}");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });
            var httpResponse = await _client.SendAsync(request);
            if (httpResponse.IsSuccessStatusCode)
            {
                var result = await httpResponse.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<AccessToken>(result,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return token;
            }
            return null;

        }

        public async Task<OrderResult> CreateOrder(Order order, AccessToken token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"{token.token_type}", $"{token.access_token}");

            var json = JsonSerializer.Serialize(order,
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _client.PostAsync("/v2/checkout/orders", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var result = await httpResponse.Content.ReadAsStringAsync();
                var orderResult = JsonSerializer.Deserialize<OrderResult>(result,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return orderResult;
            }
            return null;

        }

        public async Task<bool> CaptureOrder(AccessToken accesstoken, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"{accesstoken.token_type}", $"{accesstoken.access_token}");

            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var httpResponse = await _client.PostAsync($"v2/checkout/orders/{token}/capture", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var result = await httpResponse.Content.ReadAsStringAsync();
                return true;
            }
            return false;
        }

    }
}

