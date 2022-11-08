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

        CollectionApi<CountryApi> countriesApi = JsonConvert
            .DeserializeObject<CollectionApi<CountryApi>>(response.Content);

        return countriesApi;
    }

    public DocumentApi GetDocument(string documentKey)
    {
        RestRequest request = solexRestExecutor.GetRequest("api3/document", Method.Get);

        request.AddQueryParameter("key", documentKey);

        RestResponse response = solexRestExecutor.Execute(request);

        DocumentApi orderDocumentApi = JsonConvert.DeserializeObject<DocumentApi>(response.Content);

        return orderDocumentApi;
    }

    public CollectionPagedApi<StockInfoApi> GetStocks()
    {
        RestRequest request = solexRestExecutor.GetRequest("api3/product/findProduct", Method.Get);

        request.AddQueryParameter("field", "Ean,Qty,Sku,Units");
        request.AddQueryParameter("pageNumber", "1");
        request.AddQueryParameter("pageSize", "100");

        RestResponse response = solexRestExecutor.Execute(request);

        CollectionPagedApi<StockInfoApi> stocksApi = JsonConvert
            .DeserializeObject<CollectionPagedApi<StockInfoApi>>(response.Content);

        return stocksApi;
    }
}