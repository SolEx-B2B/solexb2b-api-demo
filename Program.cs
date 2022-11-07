using SolExB2BApiDemo.Implementation;
using SolExB2BApiDemo.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolExB2BApiDemo
{
    internal class Program
    {
        private static readonly string apiKey = "09ac8610-c60d-46a3-8f34-ae936bc3d9e2";
        private static readonly string apiUrl = "http://localhost/api2/";
        private static readonly long clientId = 1;

        public static async Task Main(string[] args)
        {
            var solexRestExecutor = new SolexRestExecutor(clientId, apiKey, apiUrl);

            var solexApi = new SolexApi(solexRestExecutor);

            CollectionPagedApi<StockInfoApi> stocksFromApi = solexApi.GetStocks();

            CollectionApi<CountryApi> countriesFromApi = solexApi.GetAvailableCountries();

            List<DocumentApi> orderDocumentsFromApi = solexApi.GetOrdersDocuments(new List<long>
            {
                111, 222, 333
            });

            PostOrderResponseApi createOrderResponseFromApi = solexApi.CreateOrder(new PostOrderApi
            {
                Address = new AddressApi
                {
                    City = "Częstochowa",
                    CountryId = 1,
                    Name = "Jan Kowalski",
                    Email = "jan@solex.net.pl",
                    Phone = "123123123",
                    Street = "Zwykła 3/4",
                    PostalCode = "42-200"
                },
                DeliveryName = "Paczkomat InPost",
                OrderLines = new PostOrderLinesApi
                {
                    KeyType = ApiProductKeyType.Ean,
                    Lines = new List<PostOrderLineApi>
                    {
                            new PostOrderLineApi
                            {
                                Key = "0123456789123",
                                Quantity = 10
                            },
                            new PostOrderLineApi
                            {
                                Key = "0987654321123",
                                Quantity = 5
                            }
                    }
                },
                Comment = "ZAMÓWIENIE TESTOWE, PROSZĘ NIE REALIZOWAĆ",
                InpostPaczkomatCode = "CZE001",
                AdditionalProperties = new List<OrderAdditionalPropertyApi>
                {
                    new OrderAdditionalPropertyApi
                    {
                        Key = "AllegroOrderId",
                        Values = new List<string>
                        {
                            "d910d1a2-f39c-4b22-bace-17a0a3e92321"
                        }
                    }
                }
            });
        }
    }
}