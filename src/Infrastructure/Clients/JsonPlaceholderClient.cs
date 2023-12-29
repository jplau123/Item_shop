using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Clients;

public class JsonPlaceholderClient : IJsonPlaceholderClient
{
    private IHttpClientFactory _httpClient;
    private readonly JsonSerializerOptions _options;

    public JsonPlaceholderClient(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;

        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<JsonPlaceholderResult<List<UserEntity>>> GetUsers()
    {
        var httpClient = _httpClient.CreateClient();

        using (var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/users", 
            HttpCompletionOption.ResponseHeadersRead))
        {
            return await CreateResult<List<UserEntity>>(response);
        }
    }

    public async Task<JsonPlaceholderResult<UserEntity>> GetUserById(int id)
    {
        var httpClient = _httpClient.CreateClient();

        using (var response = await httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}",
            HttpCompletionOption.ResponseHeadersRead))
        {
            return await CreateResult<UserEntity>(response);
        }
    }

    public async Task<JsonPlaceholderResult<UserEntity>> CreateUser(UserEntity user)
    {
        var httpClient = _httpClient.CreateClient();

        // Serialize the user object to JSON
        string userJson = JsonSerializer.Serialize(user);
        var content = new StringContent(userJson, Encoding.UTF8, "application/json");

        using (var response = await httpClient.PostAsync($"https://jsonplaceholder.typicode.com/users", content))
        {
            return await CreateResult<UserEntity>(response);
        }
    }

    private async Task<JsonPlaceholderResult<T>> CreateResult<T>(HttpResponseMessage response) where T : class
    {
        var stream = await response.Content.ReadAsStreamAsync();

        if (response.IsSuccessStatusCode)
        {
            return new JsonPlaceholderResult<T>
            {
                StatusCode = response.StatusCode,
                Data = await JsonSerializer.DeserializeAsync<T>(stream, _options),
            };
        }

        return new JsonPlaceholderResult<T>
        {
            StatusCode = response.StatusCode,
            ErrorMessage = new StreamReader(stream).ReadToEnd(),
        };
    }
}
