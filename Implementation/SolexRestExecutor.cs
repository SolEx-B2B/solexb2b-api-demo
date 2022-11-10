using RestSharp;
using SolExB2BApiDemo.Model;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SolExB2BApiDemo.Implementation;

public class SolexRestExecutor
{
    private readonly string apiKey;
    private readonly long clientId;
    private readonly RestClient restClient;

    private string token = null;
    private DateTime? tokenExpiresAt = null;

    public SolexRestExecutor(long clientId, string apiKey, string apiUrl)
    {
        this.clientId = clientId;
        this.apiKey = apiKey;
        restClient = new RestClient(apiUrl);
    }

    public RestResponse Execute(RestRequest request)
    {
        RestResponse response = restClient.Execute(request);

        return response;
    }

    public RestRequest GetRequest(string resource, Method method)
    {
        RestRequest request = new RestRequest(resource, method);

        CheckTokenStatus();

        request.AddHeader("Authorization", $"Bearer {token}");

        return request;
    }

    private static string GenerateAuthMd5(DateTime dateTime, string userApiKey, long userId)
    {
        string stringHashInput = userApiKey.ToUpper() + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + userId;

        byte[] bytesHashInput = Encoding.UTF8.GetBytes(stringHashInput);

        string hash = GetHash(bytesHashInput);

        return hash;
    }

    private static string GetHash(byte[] bytes)
    {
        using (MD5 md5Hasher = MD5.Create())
        {
            byte[] data = md5Hasher.ComputeHash(bytes);

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2").ToLower());
            }

            md5Hasher.Dispose();

            return stringBuilder.ToString();
        }
    }

    private static bool IsTokenValid(string token, DateTime? tokenExpiresAt)
    {
        if (!string.IsNullOrWhiteSpace(token) && tokenExpiresAt.HasValue
            && ((DateTime)tokenExpiresAt).AddMinutes(-2) > DateTime.UtcNow)
        {
            return true;
        }

        return false;
    }

    private void CheckTokenStatus()
    {
        if (IsTokenValid(token, tokenExpiresAt))
        {
            return;
        }

        RestRequest request = new RestRequest("api3/token", Method.Post);

        DateTime timestamp = DateTime.UtcNow;

        string hash = GenerateAuthMd5(timestamp, apiKey, clientId);

        request.AddJsonBody(new
        {
            Hash = hash,
            ClientId = clientId,
            Timestamp = timestamp
        });

        RestResponse response = Execute(request);

        TokenApi apiToken = JsonSerializer.Deserialize<TokenApi>(response.Content);

        tokenExpiresAt = DateTime.UtcNow.AddSeconds(apiToken.ExpiresIn);
        token = apiToken.AccessToken;
    }
}