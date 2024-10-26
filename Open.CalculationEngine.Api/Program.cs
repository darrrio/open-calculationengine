using Microsoft.OpenApi.Models;
using Open.CalculationEngine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IncludeFields = true;
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Open CalculationEngine API",
        Description = "An ASP.NET Core Web API for managing mathematics calculations.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});
builder.Services.AddScoped<Context>();
builder.Services.AddScoped<ExpressionNode>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();   

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
