using FluentValidation.AspNetCore;
using log4net.Config;
using TransactionAPI.Interfaces;
using TransactionAPI.Models.Configuration;
using TransactionAPI.Models.Validation;
using TransactionAPI.Services;

var builder = WebApplication.CreateBuilder(args);

XmlConfigurator.Configure(new FileInfo("log4net.config"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddSingleton<ILoggingService, LoggingService>();

builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();