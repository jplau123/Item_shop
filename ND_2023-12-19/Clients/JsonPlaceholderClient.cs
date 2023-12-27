using ND_2023_12_19.DTOs;
using System.Text.Json;

namespace ND_2023_12_19.Clients;

public class JsonPlaceholderClient
{
    private IHttpClientFactory _httpClient;
    private readonly JsonSerializerOptions _options;

    public JsonPlaceholderClient(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;

        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    public async Task<List<UserDto>?> GetUsers()
    {
        var httpClient = _httpClient.CreateClient();

        using (var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/users", HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<UserDto>>(stream, _options);
        }
    }
    
    public async Task<UserDto?> GetUserById(int id)
    {
        var httpClient = _httpClient.CreateClient();

        using (var response = await httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}", HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<UserDto>(stream, _options);
        }
    }
    
    //public async Task<UserDto?> Create(UserDto)
    //{
    //    var httpClient = _httpClient.CreateClient();

    //    using (var response = await httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}", HttpCompletionOption.ResponseHeadersRead))
    //    {
    //        response.EnsureSuccessStatusCode();
    //        var stream = await response.Content.ReadAsStreamAsync();
    //        return await JsonSerializer.DeserializeAsync<UserDto>(stream, _options);
    //    }
    //}
}
