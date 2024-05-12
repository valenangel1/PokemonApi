namespace entregabletres.src.Handler;

using entregabletres.src.Config;
using entregabletres.src.Entity;
using System.Collections.Generic;
public class GetAllPokemonHandler
{
    private DbMemory _db;

    internal GetAllPokemonHandler(
        DbMemory db
    ){
        this._db = db;
    }

    public IEnumerable<Pokemon> Handle()
    {
        return this._db.Pokemon.ToList<Pokemon>();
    }
}