using System.Text.Json;
using DiscountService.Models;

namespace DiscountService.Services;

public class DiscountServiceClient
{
    private readonly HttpClient _client;

    public DiscountServiceClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<DiscountResponse> GetDiscountAsync(int productId)
    {
        var response = await _client.GetAsync($"/api/discounts/{productId}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStreamAsync();
        return JsonSerializer.Deserialize<DiscountResponse>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;
    }
}