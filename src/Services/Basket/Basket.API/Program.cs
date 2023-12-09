using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Transports.Fabric;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    options => options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
    });
});

builder.Services.AddCors(options =>
{
    var apiGw = builder.Configuration.GetValue<string>("CorsPolicy:OcelotApiGwUrl");
    var shoppingAggregator = builder.Configuration.GetValue<string>("CorsPolicy:ShoppingAggregatorUrl");
    options.AddPolicy($"Development",
        builder => builder.WithOrigins(apiGw, shoppingAggregator)
                          .WithMethods("GET", "PUT", "POST", "DELETE")
                          .AllowAnyHeader());
    options.AddPolicy($"Production",
        builder => builder.WithOrigins(apiGw, shoppingAggregator)
                          .WithMethods("GET", "PUT", "POST", "DELETE")
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors($"{app.Environment.EnvironmentName}");
}

app.UseAuthorization();

app.MapControllers();

app.Run();
