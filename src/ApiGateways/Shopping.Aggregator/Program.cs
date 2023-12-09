using Microsoft.OpenApi.Models;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping.Aggregator", Version = "v1" });
});
builder.Services.AddHttpClient<ICatalogService, CatalogService>(http =>
{
    var uriString = builder.Configuration.GetValue<string>("ApiSettings:CatalogUrl");
    http.BaseAddress = new Uri(uriString);
});
builder.Services.AddHttpClient<IBasketService, BasketService>(http =>
{
    var uriString = builder.Configuration.GetValue<string>("ApiSettings:BasketUrl");
    http.BaseAddress = new Uri(uriString);
});
builder.Services.AddHttpClient<IOrderService, OrderService>(http =>
{
    var uriString = builder.Configuration.GetValue<string>("ApiSettings:OrderingUrl");
    http.BaseAddress = new Uri(uriString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
