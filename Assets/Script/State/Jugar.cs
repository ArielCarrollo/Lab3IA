using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Jugar : Humano
{
    private void Awake()
    {
        typestate = TypeState.Jugar;
        LocadComponent();
    }

    public override void Execute()
    {
        if (_DataAgent.IsMoving) // Solo ejecutar si no está en movimiento
        {
            base.Execute();
            return;
        }

        // Consumir energía
        _DataAgent.DiscountEnergy();

        // Consumir sueño más rápido al jugar
        if (!_DataAgent.IsInBathroom)
        {
            _DataAgent.Sleep.value = Mathf.Max(0, _DataAgent.Sleep.value - Time.deltaTime * 0.02f);
        }

        // Consumir necesidad de baño más rápido al jugar
        _DataAgent.WC.value = Mathf.Max(0, _DataAgent.WC.value - Time.deltaTime * 0.03f);

        // Verificar prioridades
        if (_DataAgent.WC.value < 0.15f)
        {
            _StateMachine.ChangeState(TypeState.Banno);
            MoveToNewLocation(Location.Banno);
        }
        else if (_DataAgent.Energy.value < 0.25f)
        {
            _StateMachine.ChangeState(TypeState.Comer);
            MoveToNewLocation(Location.Comedor);
        }
        else if (_DataAgent.Sleep.value < 0.2f && !_DataAgent.IsInBathroom)
        {
            _StateMachine.ChangeState(TypeState.Dormir);
            MoveToNewLocation(Location.Dormitorio);
        }

        base.Execute();
    }

    private void MoveToNewLocation(Location newLocation)
    {
        var route = PathMap.routes[(Location.SalaDeJuegos, newLocation)];
        List<Transform> transforms = route.Select(loc => WaypointManager.Instance.GetWaypoint(loc)).ToList();

        // Iniciar movimiento hacia el nuevo destino
        _Movement.FollowPath(transforms);
    }
}

