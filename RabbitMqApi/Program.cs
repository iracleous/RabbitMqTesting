using RabbitMqApi.Data;
using RabbitMqApi.Rabbit;
using RabbitMqApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddScoped<IRabbitMqProducer, RabbitMqProducer>();


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
