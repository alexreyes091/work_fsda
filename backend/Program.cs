using app.webapi.backoffice_viajes_altairis.Data;
using app.webapi.backoffice_viajes_altairis.Domain.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

DotNetEnv.Env.TraversePath().Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Validators from FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RoomValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<HotelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ReservationValidator>();

//Configuration db
var connectionString = builder.Configuration["CONNECTION_STRING"];
builder.Services.AddDbContext<AltarisDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.MapScalarApiReference( options =>
    {
        options
            .WithTitle("Altaris")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.Run();