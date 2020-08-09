using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolExB2BApiDemo
{
    class Program
    {
        private static HttpClient client = null;
        private static string SecretApiKey = "09ac8610-c60d-46a3-8f34-ae936bc3d9e2"; //secret api key

        public static async Task Main(string[] args)
        {
            SetupClient();

            LogOut();

            return;
        }

        private static async Task<string> LoginToApi(string ApiKey)
        {
            var urlToLogin = $"token?client_id={ ApiKey }";

            Console.WriteLine($"GET url: {client.BaseAddress + urlToLogin} ");

            var result = await client.GetAsync(urlToLogin);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {await result.Content.ReadAsStringAsync()}");
            }

            var token = await JsonSerializer.DeserializeAsync<Token>(await result.Content.ReadAsStreamAsync());

            Console.WriteLine($"Token: {token}");

            return token.access_token;
        }

        private static async void LogOut()
        {
            var result = await client.DeleteAsync("token");

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {await result.Content.ReadAsStringAsync()}");
            }
        }


        private static async void SetupClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://demo.solexb2b.com/api2/");

            //login to API
            string token = await LoginToApi(SecretApiKey);

            client.DefaultRequestHeaders.Authorization = new  AuthenticationHeaderValue("Bearer", token);
        }

    
   

    }
}
