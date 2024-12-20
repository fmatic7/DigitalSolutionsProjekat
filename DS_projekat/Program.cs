using DS_projekat.Models;
using DS_projekat.Properties;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

//Omogucava se API dokumentacija pomocu Swagger-a
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Omogucava se da FluentValidation automatski primeni validaciju na modele i olaksava rad s validacijom podataka 
builder.Services.AddValidatorsFromAssemblyContaining<PosiljkaValidator>();
builder.Services.AddFluentValidationAutoValidation();


var app = builder.Build();

var posiljke = new List<Posiljka>();

//Omogucava se da se Swagger UI i Swagger dokumentacija aktiviraju samo u development modu, kako bi olaksale razvoj i testiranje aplikacije
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//POST metoda
app.MapPost("/posiljke", (Posiljka novaPosiljka) =>
{
    novaPosiljka.Id = Guid.NewGuid();
    novaPosiljka.DatumKreiranja = DateTime.UtcNow;
    posiljke.Add(novaPosiljka);

    return Results.Created($"/posiljke/{novaPosiljka.Id}", novaPosiljka);
});

//GET metoda za jednu posiljku
app.MapGet("/posiljke/{id:guid}", (Guid id) =>
{
    var posiljka = posiljke.FirstOrDefault(p => p.Id == id);
    if (posiljka == null)
    {
        return Results.NotFound("Posiljka nije pronadjena.");
    }

    return Results.Ok(posiljka);
});

//GET metoda za sve posiljke
app.MapGet("/posiljke", () => posiljke);

//PUT metoda
app.MapPut("/posiljke/{id:guid}", (Guid id, Posiljka novaPosiljka) =>
{
    var posiljka = posiljke.FirstOrDefault(p => p.Id == id);
    if (posiljka == null)
    {
        return Results.NotFound("Posiljka nije pronadjena.");
    }

    posiljka.Naziv = novaPosiljka.Naziv;
    posiljka.Status = novaPosiljka.Status;
    posiljka.DatumIsporuke = novaPosiljka.DatumIsporuke;

    return Results.Ok(posiljka);
});

//DELETE metoda
app.MapDelete("/posiljke/{id:guid}", (Guid id) =>
{
    var posiljka = posiljke.FirstOrDefault(p => p.Id == id);
    if (posiljka == null)
    {
        return Results.NotFound("Posiljka nije pronadjena.");
    }

    posiljke.Remove(posiljka);
    return Results.Ok("Posiljka je uspesno obrisana.");
});

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.Run();

//https://localhost:7222;