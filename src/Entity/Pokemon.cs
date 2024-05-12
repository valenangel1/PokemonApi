namespace entregabletres.src.Entity;

using System.ComponentModel.DataAnnotations;

public class Pokemon
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Tipo { get; set; }
    public int ataque1 { get; set; }
    public int ataque2 { get; set; }
    public int ataque3 { get; set; }
    public int ataque4 { get; set; }

    public int Defensa { get; set; }


}