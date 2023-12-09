using Catalog.API.Data;
using Catalog.API.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
});
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
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
}else
{
    app.UseCors($"{app.Environment.EnvironmentName}");
}

app.UseAuthorization();

app.MapControllers();

app.Run();
