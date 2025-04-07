using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Humano : State
{
    public DataAgent _DataAgent;
    public AgentMovement2D _Movement;
    public Location CurrentLocation;  

    public override void LocadComponent()
    {
        base.LocadComponent();
        _DataAgent = GetComponent<DataAgent>();
        _Movement = GetComponent<AgentMovement2D>(); 
    }
}
