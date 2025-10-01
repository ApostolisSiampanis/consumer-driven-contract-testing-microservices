using DiscountSvc_Provider.Models;
using DiscountSvc_Provider.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<DiscountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection(); 
app.UseAuthorization();
app.MapControllers();
app.UseRouting();

// Minimal API endpoint replacing UseEndpoints + manual service resolution
app.MapPost("/discount", (DiscountModel model, DiscountService svc) =>
{
    var amount = svc.GetDiscountAmount(model.CustomerRating);
    return Results.Json(new DiscountModel
    {
        CustomerRating = model.CustomerRating,
        AmountToDiscount = amount
    });
});

app.Run();