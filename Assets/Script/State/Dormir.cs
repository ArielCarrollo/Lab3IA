using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dormir : Humano
{
    private float sleepRecoveryRate = 0.1f;
    private float energyRecoveryRate = 0.05f;
    private bool movingToBed = true;

    private void Awake()
    {
        typestate = TypeState.Dormir;
        LocadComponent();
    }

    public override void Enter()
    {
        _DataAgent.IsSleeping = false;
        movingToBed = true;

        // Aseg�rate de que CurrentLocation est� asignado correctamente antes de usar esto
        var route = PathMap.routes[(CurrentLocation, Location.Dormitorio)];
        List<Transform> transforms = route.Select(loc => WaypointManager.Instance.GetWaypoint(loc)).ToList();

        _Movement.FollowPath(transforms);
    }

    public override void Execute()
    {
        if (movingToBed)
        {
            if (_Movement.IsDone())
            {
                movingToBed = false;
                _DataAgent.IsSleeping = true;
            }
            base.Execute();
            return;
        }

        // EXCEPCI�N: La energ�a SIEMPRE se recupera durante el sue�o
        _DataAgent.Energy.value = Mathf.Min(1f, _DataAgent.Energy.value + energyRecoveryRate * Time.deltaTime);

        // El sue�o solo se recupera si no est� en movimiento
        if (!_DataAgent.IsMoving)
        {
            _DataAgent.Sleep.value = Mathf.Min(1f, _DataAgent.Sleep.value + sleepRecoveryRate * Time.deltaTime);
        }

        if (_DataAgent.Sleep.value >= 0.95f)
        {
            if (_DataAgent.WC.value < 0.2f)
            {
                _StateMachine.ChangeState(TypeState.Banno);
                MoveToNewLocation(Location.Banno);
            }
            else if (_DataAgent.Energy.value < 0.5f)
            {
                _StateMachine.ChangeState(TypeState.Comer);
                MoveToNewLocation(Location.Comedor);
            }
            else
            {
                _StateMachine.ChangeState(TypeState.Jugar);
                MoveToNewLocation(Location.SalaDeJuegos);
            }
        }

        base.Execute();
    }

    private void MoveToNewLocation(Location newLocation)
    {
        // Obtener la ruta correspondiente para el nuevo destino
        var route = PathMap.routes[(Location.Dormitorio, newLocation)];
        List<Transform> transforms = route.Select(loc => WaypointManager.Instance.GetWaypoint(loc)).ToList();

        // Iniciar movimiento hacia el nuevo destino
        _Movement.FollowPath(transforms);
    }


    public override void Exit()
    {
        _DataAgent.IsSleeping = false;
    }
}
