using Newtonsoft.Json;
using RestSharp;
using SolExB2BApiDemo.Model;
using System.Collections.Generic;

namespace SolExB2BApiDemo.Implementation;

public class SolexApi
{
    private readonly SolexRestExecutor solexRestExecutor;

    public SolexApi(SolexRestExecutor solexRestExecutor)
    {
        this.solexRestExecutor = solexRestExecutor;
    }

    public PostOrderResponseApi CreateOrder(PostOrderApi order)
    {
        RestRequest request = solexRestExecutor.GetRequest("api3/order", Method.Post);

        request.AddJsonBody(order);

        RestResponse response = solexRestExecutor.Execute(request);

        PostOrderResponseApi postOrderResponseApi = JsonConvert
            .DeserializeObject<PostOrderResponseApi>(response.Content);

        return postOrderResponseApi;
    }

    public CollectionApi<CountryApi> GetAvailableCountries()
    {
        RestRequest request = solexRestExecutor.GetRequest("api3/address/country", Method.Get);

        RestResponse response = solexRestExecutor.Execute(request);

        CollectionApi<CountryApi> countries = JsonConvert
            .DeserializeObject<CollectionApi<CountryApi>>(response.Content);

        return countries;
    }

    public List<DocumentApi> GetOrdersDocuments(List<long> solexOrdersIds)
    {
        RestRequest request = solexRestExecutor.GetRequest("api3/document/filter", Method.Post);

        request.AddJsonBody(new GetFilteredDocumentsApi
        {
            B2BIds = solexOrdersIds
        });

        RestResponse response = solexRestExecutor.Execute(request);

        CollectionApi<DocumentApi> ordersDocumentsApi = JsonConvert
            .DeserializeObject<CollectionApi<DocumentApi>>(response.Content);

        return ordersDocumentsApi.Items;
    }

    public CollectionPagedApi<StockInfoApi> GetStocks()
    {
        RestRequest request = solexRestExecutor.GetRequest("api3/product/findProduct", Method.Get);

        request.AddQueryParameter("field", "Ean,Qty,Sku,Units");

        RestResponse response = solexRestExecutor.Execute(request);

        CollectionPagedApi<StockInfoApi> stocksApi = JsonConvert
            .DeserializeObject<CollectionPagedApi<StockInfoApi>>(response.Content);

        return stocksApi;
    }
}