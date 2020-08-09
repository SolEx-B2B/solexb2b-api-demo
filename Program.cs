using System;
using System.Collections.Generic;
using System.Linq;
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
        private static string UrlDemoB2B = "http://localhost/api2/";

        public static async Task Main(string[] args)
        {
            await SetupClient();

            await GetAllProducts();

            await GetProductsSearch();


            Dictionary<long, string> idOrder = await GetOrders();

            await GetOrderStatusById(idOrder.First().Key);
            await GetOrderStatusByName(idOrder.First().Value);
            
            await LogOut();

            //try to use old token - should thro exception

            Console.WriteLine("We deleted token, so cannot use it again");

            await GetAllProducts();

            return;
        }

        private static async Task GetOrderStatusById(long idOrder)
        {
            var result = await client.GetAsync($"order/status/{idOrder}");

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                throw new Exception($"Error: {responseContent}");
            }

           OrderStatus orderStatus = await JsonSerializer.DeserializeAsync<OrderStatus>(await result.Content.ReadAsStreamAsync());

           Console.WriteLine($"Status of order #{idOrder}: { JsonSerializer.Serialize(orderStatus) }");
        }


        private static async Task GetOrderStatusByName(string orderName)
        {
            var result = await client.GetAsync($"order/status/{orderName}");

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                throw new Exception($"Error: {responseContent}");
            }

            OrderStatus orderStatus = await JsonSerializer.DeserializeAsync<OrderStatus>(await result.Content.ReadAsStreamAsync());

            Console.WriteLine($"Status of order name {orderName}: { JsonSerializer.Serialize(orderStatus) }");
        }


        private static async Task< Dictionary<long, string>> GetOrders()
        {
            Console.WriteLine("Getting orders from last 999 days");

            var result = await client.GetAsync("order/all/999");

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                throw new Exception($"Error: {responseContent}");
            }

            Dictionary<long, string> ordersIds = await JsonSerializer.DeserializeAsync<Dictionary<long, string>>(await result.Content.ReadAsStreamAsync());

            Console.WriteLine($"We have {ordersIds.Count} orders in last 999 days - first: {ordersIds.First().Key} - {ordersIds.First().Value}");

           return ordersIds;
        }





        private static async Task GetAllProducts()
        {
            var result = await client.GetAsync("product/findProduct?field=name,ean,code&PageSize=30&PageNumber=1");

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                throw new Exception($"Error: {responseContent}");
            }

            Products products = await JsonSerializer.DeserializeAsync<Products>(await result.Content.ReadAsStreamAsync());
            Console.WriteLine($"We have: {products.count} products on B2B, but now we download only: {products.items.Length}");
            Console.WriteLine( $"First product name: {products.items[0].name} - desc: {products.items[0].description} - qty: {products.items[0].qty}"  );
        }

        private static async Task GetProductsSearch()
        {
            var result = await client.GetAsync("product/findProduct?field=name,ean,code&where=black 50ml&PageSize=30&PageNumber=1");

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                throw new Exception($"Error: {responseContent}");
            }

            Products products = await JsonSerializer.DeserializeAsync<Products>(await result.Content.ReadAsStreamAsync());
            Console.WriteLine($"We found: {products.count} products for 'black 50ml'");
            Console.WriteLine( $"First product name: {products.items[0].name} - desc: {products.items[0].description} - qty: {products.items[0].qty}"  );
        }


        private static async Task<string> LoginToApi(string ApiKey)
        {
            var urlToLogin = $"token?client_id={ ApiKey }";

            Console.WriteLine($"GET url: {client.BaseAddress + urlToLogin} ");

            var result = await client.GetAsync(urlToLogin);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                throw new Exception($"Error: {responseContent}");
            }

            Token token = await JsonSerializer.DeserializeAsync<Token>(await result.Content.ReadAsStreamAsync());

            Console.WriteLine($"Token: {token.access_token}");

            return token.access_token;
        }

        private static async Task LogOut()
        {
            var result = await client.DeleteAsync("token");

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {await result.Content.ReadAsStringAsync()}");
            }
        }


        private static async Task SetupClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(UrlDemoB2B);

            //login to API
            string token = await LoginToApi(SecretApiKey);

            client.DefaultRequestHeaders.Authorization = new  AuthenticationHeaderValue("Bearer", token);
        }

    
   

    }
}
