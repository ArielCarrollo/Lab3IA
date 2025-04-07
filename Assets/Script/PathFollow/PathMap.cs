using System.Collections.Generic;

public enum Location
{
    SalaDeJuegos,
    Dormitorio,
    Escaleras1,
    Comedor,
    Escaleras2,
    Banno
}

public static class PathMap
{
    public static Dictionary<(Location, Location), List<Location>> routes = new Dictionary<(Location, Location), List<Location>>()
    {
        // Sala de juegos
        {(Location.SalaDeJuegos, Location.Dormitorio), new List<Location>{ Location.Dormitorio }},
        {(Location.SalaDeJuegos, Location.Comedor), new List<Location>{ Location.Dormitorio, Location.Escaleras1, Location.Comedor }},
        {(Location.SalaDeJuegos, Location.Banno), new List<Location>{ Location.Dormitorio, Location.Escaleras1, Location.Comedor, Location.Escaleras2, Location.Banno }},
        // Dormitorio
        {(Location.Dormitorio, Location.SalaDeJuegos), new List<Location>{ Location.SalaDeJuegos }},
        {(Location.Dormitorio, Location.Comedor), new List<Location>{ Location.Escaleras1, Location.Comedor }},
        {(Location.Dormitorio, Location.Banno), new List<Location>{ Location.Escaleras1, Location.Comedor, Location.Escaleras2, Location.Banno }},
        //Comedor
        {(Location.Comedor, Location.SalaDeJuegos), new List<Location>{ Location.Escaleras1, Location.Dormitorio, Location.SalaDeJuegos }},
        {(Location.Comedor, Location.Dormitorio), new List<Location>{ Location.Escaleras1, Location.Dormitorio, }},
        {(Location.Comedor, Location.Banno), new List<Location>{ Location.Escaleras2, Location.Banno }},
        //Banno
        {(Location.Banno, Location.SalaDeJuegos), new List<Location>{ Location.Escaleras2, Location.Comedor, Location.Escaleras1, Location.Dormitorio, Location.SalaDeJuegos }},
        {(Location.Banno, Location.Dormitorio), new List<Location>{ Location.Escaleras2, Location.Comedor, Location.Escaleras1, Location.Dormitorio }},
        {(Location.Banno, Location.Comedor), new List<Location>{ Location.Escaleras2, Location.Comedor }},
    };
}
