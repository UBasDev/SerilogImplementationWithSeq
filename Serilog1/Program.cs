using Microsoft.AspNetCore.HttpLogging;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Serilog1.Interceptors;
using Serilog1.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
   
});

// Add services to the container.
//builder.Services.AddHttpLogging(logging => { logging.LoggingFields = HttpLoggingFields.All; });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<SerilogMiddleware1>();
app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
