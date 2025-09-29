using System.Net;
using System.Net.Http.Json;
using OrderSvc_Consumer.Models;
using PactNet;

namespace OrdersService.Tests;

public class DiscountSvcTests
{
    [Fact]
    public async Task GetDiscountAdjustmentAmount()
    {
        var pact = Pact.V2("Orders", "Discounts", new PactConfig
        {
            PactDir = Path.Combine("..", "..", "..", "pact"),
            LogLevel = PactLogLevel.Debug
        });

        var api = pact.WithHttpInteractions();

        var discountModel = new DiscountModel { CustomerRating = 4.1 };

        api
            .UponReceiving("Give a customer rating, an adjustment discount amount will be returned.")
                .Given("Rate")
                .WithRequest(HttpMethod.Post, "/discount")
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new { customerRating = 4.1, amountToDiscount = 0.0 })
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new { customerRating = 4.1, amountToDiscount = .41 });

        await api.VerifyAsync(async ctx =>
        {
            using var httpClient = new HttpClient { BaseAddress = ctx.MockServerUri };
            var response = await httpClient.PostAsJsonAsync("/discount", discountModel);
            var body = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, $"Status: {(int)response.StatusCode} Body: {body}");
            var discountModelReturned = await response.Content.ReadFromJsonAsync<DiscountModel>();
            Assert.NotNull(discountModelReturned);
            Assert.Equal(discountModel.CustomerRating, discountModelReturned!.CustomerRating);
            Assert.Equal(.41, discountModelReturned.AmountToDiscount);
        });
    }
}