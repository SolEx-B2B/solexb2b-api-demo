using SolExB2BApiDemo.Implementation;
using SolExB2BApiDemo.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolExB2BApiDemo
{
    internal class Program
    {
        private static readonly string apiKey = "09ac8610-c60d-46a3-8f34-ae936bc3d9e2";
        private static readonly string apiUrl = "http://demo.solexb2b.com";
        private static readonly long clientId = 1;

        public static async Task Main(string[] args)
        {
            var solexRestExecutor = new SolexRestExecutor(clientId, apiKey, apiUrl);

            var solexApi = new SolexApi(solexRestExecutor);

            CollectionPagedApi<StockInfoApi> stocksFromApi = solexApi.GetStocks();

            CollectionApi<CountryApi> countriesFromApi = solexApi.GetAvailableCountries();

            DocumentApi documentFromApi = solexApi.GetDocument("5868");

            PostOrderResponseApi createOrderResponseFromApi = solexApi.CreateOrder(new PostOrderApi
            {
                Address = new AddressApi
                {
                    City = "Częstochowa",
                    CountryId = 1,
                    Name = "Jan Kowalski",
                    Email = "jan@solex.net.pl",
                    Phone = "+48123123123",
                    Street = "Zwykła 3/4",
                    PostalCode = "42-200"
                },
                DeliveryName = "Odbiór osobisty",
                OrderLines = new PostOrderLinesApi
                {
                    KeyType = ApiProductKeyType.Id,
                    Lines = new List<PostOrderLineApi>
                    {
                        new PostOrderLineApi
                        {
                            Key = "871",
                            Quantity = 10
                        },
                        new PostOrderLineApi
                        {
                            Key = "84534",
                            Quantity = 5
                        }
                    }
                },
                Comment = "ZAMÓWIENIE TESTOWE, PROSZĘ NIE REALIZOWAĆ",
                AdditionalProperties = new List<OrderAdditionalPropertyApi>
                {
                    new OrderAdditionalPropertyApi
                    {
                        Key = "Uwagi ZK",
                        Values = new List<string>
                        {
                            "ZAMÓWIENIE TESTOWE, PROSZĘ NIE REALIZOWAĆ"
                        }
                    }
                }
            });
        }
    }
}