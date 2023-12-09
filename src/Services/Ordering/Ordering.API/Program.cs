using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.API.EventBusConsumers;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));

        cfg.ReceiveEndpoint(EventBusConstants.BASKET_CHECKOUT_QUEUE, cfg =>
        {
            cfg.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
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
app.MigrateDatabase<OrderContext>((context, serviceProvider) =>
{
    var logger = serviceProvider.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();

});

app.UseAuthorization();

app.MapControllers();

app.Run();
