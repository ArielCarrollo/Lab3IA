using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance;

    public Transform SalaDeJuegos, Dormitorio, Escaleras1, Comedor, Escaleras2, Banno;

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetWaypoint(Location location)
    {
        return location switch
        {
            Location.SalaDeJuegos => SalaDeJuegos,
            Location.Dormitorio => Dormitorio,
            Location.Escaleras1 => Escaleras1,
            Location.Comedor => Comedor,
            Location.Escaleras2 => Escaleras2,
            Location.Banno => Banno,
            _ => null
        };
    }
}

