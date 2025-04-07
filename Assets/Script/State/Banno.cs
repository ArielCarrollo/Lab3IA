using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Banno : Humano
{
    private float bathroomTime = 3f;
    private float timer = 0f;

    private void Awake()
    {
        typestate = TypeState.Banno;
        LocadComponent();
    }

    public override void Enter()
    {
        timer = 0f;
        _DataAgent.IsInBathroom = true; // Marcamos que está en el baño
        // Iniciar animación de ir al baño
    }

    public override void Execute()
    {
        if (_DataAgent.IsMoving || !_Movement.IsDone()) // Solo ejecutar si llegó al baño
        {
            base.Execute();
            return;
        }

        _DataAgent.WC.value = Mathf.Min(1f, _DataAgent.WC.value + 0.1f * Time.deltaTime);

        if (_DataAgent.WC.value >= 0.95f)
        {
            _DataAgent.IsInBathroom = false;
            _DataAgent.WC.value = 1f;

            if (_DataAgent.Energy.value < 0.3f)
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

    public override void Exit()
    {
        _DataAgent.IsInBathroom = false; // Por si acaso se sale de otro modo
        // Finalizar animación de baño
    }

    private void MoveToNewLocation(Location newLocation)
    {
        var route = PathMap.routes[(Location.Banno, newLocation)];
        List<Transform> transforms = route.Select(loc => WaypointManager.Instance.GetWaypoint(loc)).ToList();

        // Iniciar movimiento hacia el nuevo destino
        _Movement.FollowPath(transforms);
    }
}


