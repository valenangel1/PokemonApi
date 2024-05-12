using Microsoft.EntityFrameworkCore;
using entregabletres.src.Config;
using entregabletres.src.Entity;
using entregabletres.src.Handler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbMemory>(opt => opt.UseInMemoryDatabase("Pokemonlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/pokemon", (DbMemory db) => {
    try
    {
        GetAllPokemonHandler handler = new GetAllPokemonHandler(db);
        var pokemons = handler.Handle();
        return Results.Ok(pokemons);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
    
});

app.MapPost("/pokemon", async (HttpContext context, DbMemory db) =>
{   
    try {
    Pokemon? pokemon = await context.Request.ReadFromJsonAsync<Pokemon>();
    if (pokemon == null ||  pokemon.Defensa < 1 || pokemon.Defensa > 30 || pokemon.ataque1 < 0 || pokemon.ataque1 >40 ||
    pokemon.ataque2 < 0 || pokemon.ataque2 >40 || pokemon.ataque3 < 0 || pokemon.ataque3 >40 || pokemon.ataque4 < 0 || pokemon.ataque4 >40)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }
    CreatePokemonHandler handler = new CreatePokemonHandler(db);
    await handler.HandleAsync(pokemon);
    } catch (Exception e){
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(e.Message);
        return;
    }
    
});

app.MapPut("/pokemon/{id:int}", async(int id, Pokemon pokemonUpdate, DbMemory db) => {
    var pokemon = await db.Pokemon.FindAsync(id);

    if (pokemon == null ||  pokemon.Defensa < 1 || pokemon.Defensa > 30 || pokemon.ataque1 < 0 || pokemon.ataque1 >40 ||
    pokemon.ataque2 < 0 || pokemon.ataque2 >40 || pokemon.ataque3 < 0 || pokemon.ataque3 >40 || pokemon.ataque4 < 0 || pokemon.ataque4 >40) {
        return Results.NotFound();
    }


    db.Entry(pokemon).CurrentValues.SetValues(pokemonUpdate);
    await db.SaveChangesAsync();
    return Results.Ok(pokemon);
});

app.MapDelete("/pokemon/{id:int}", async(int id, DbMemory db) => {
    var pokemon = await db.Pokemon.FindAsync(id);

    if (pokemon == null) {
        return Results.NotFound();
    }

    db.Pokemon.Remove(pokemon);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("/pokemon/tipo/{tipo}", async(string tipo, DbMemory db) => {
    try {
        var pokemon = db.Pokemon.Where(p => p.Tipo == tipo).ToList();
        return Results.Ok(pokemon);
    } catch (Exception e) {
        return Results.Problem(e.Message);
    }
});

app.MapPost("/pokemon/varios", async(HttpContext context, DbMemory db) =>
{   
    try {
        List<Pokemon> pokemones = await context.Request.ReadFromJsonAsync<List<Pokemon>>();

        if (pokemones == null || pokemones.Count == 0) {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("No hay pokemons");
            return;
        }
        foreach (var pokemon in pokemones) {
            db.Pokemon.Add(pokemon);
        }
        await db.SaveChangesAsync();
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync("Pokémones añadidos");
    } catch (Exception e){
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(e.Message);
        return;
    }   
});
app.Run();
