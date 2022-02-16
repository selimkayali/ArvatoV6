using ArvatoV6.Library.Services.Abstract;
using ArvatoV6.Library.Services.Concrete;
using ArvatoV6.Validations;
using FluentValidation;
using ArvatoV6.Models.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICardValidationService, CardValidationService>();
builder.Services.AddSingleton<IValidator<CreditCardInputDto>, CreditCardValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

