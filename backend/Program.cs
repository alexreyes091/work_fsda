using app.webapi.backoffice_viajes_altairis.Data;
using app.webapi.backoffice_viajes_altairis.Data.Interfaces;
using app.webapi.backoffice_viajes_altairis.Data.Repository;
using app.webapi.backoffice_viajes_altairis.Data.Seeder;
using app.webapi.backoffice_viajes_altairis.Domain.Validators;
using app.webapi.backoffice_viajes_altairis.Endpoints;
using app.webapi.backoffice_viajes_altairis.Services;
using app.webapi.backoffice_viajes_altairis.Services.Interfaces;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

DotNetEnv.Env.TraversePath().Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Validators from FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RoomValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<HotelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ReservationValidator>();
// Repositories
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
// Services
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRoomService, RoomService>();

//Configuration db
var connectionString = builder.Configuration["CONNECTION_STRING"];
builder.Services.AddDbContext<AltarisDbContext>(options => options.UseNpgsql(connectionString));
// Mapper
builder.Services.AddSingleton<IMapper, Mapper>();
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference( options =>
    {
        options
            .WithTitle("Altaris")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
// Aplicando migraciones y seeders
await app.ApplyMigrationsAndSeedAsync();

app.UseCors();
app.UseHttpsRedirection();
app.MapHotelEndpoints();
app.MapRoomEndpoints();

app.Run();