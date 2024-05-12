namespace entregabletres.src.Handler;
using Microsoft.AspNetCore.Mvc;
using entregabletres.src.Config;
using entregabletres.src.Entity;

public class CreatePokemonHandler
{
    private DbMemory _db;

    internal CreatePokemonHandler(DbMemory db){
        this._db = db;
    }

    public async Task<IActionResult> HandleAsync(Pokemon pokemon)
    {
    
        this._db.Pokemon.Add(pokemon);
        await this._db.SaveChangesAsync();

        return new CreatedResult($"/pokemon_post/", pokemon);
    }
}