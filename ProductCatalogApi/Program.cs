using Microsoft.OpenApi.Models;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ProdutoDatabaseSettings>(builder.Configuration.GetSection("ProdutoDatabase"));

builder.Services.AddSingleton<ProdutoServices>();
builder.Services.AddSingleton<CategoriaServices>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo {
            Version = "v1",
            Title = "ProductCatalogApi",
            Description = "API para gestão de categorias e produtos",
            Contact = new OpenApiContact {
                Name = "Github",
                Url = new Uri("https://github.com/cassiohirota/produto-rest-api")
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
