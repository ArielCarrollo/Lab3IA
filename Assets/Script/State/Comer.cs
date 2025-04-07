using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Comer : Humano
{
    private float eatingTime = 5f; // Tiempo que tarda en comer
    private float timer = 0f;

    private void Awake()
    {
        typestate = TypeState.Comer;
        LocadComponent();
    }

    public override void Enter()
    {
        timer = 0f;
        // Puedes agregar aquí animaciones o efectos de comer
    }

    public override void Execute()
    {
        if (_DataAgent.IsMoving || !_Movement.IsDone()) // Solo ejecutar si llegó al comedor
        {
            base.Execute();
            return;
        }

        timer += Time.deltaTime;
        _DataAgent.Energy.value = Mathf.Lerp(_DataAgent.Energy.value, 1f, Time.deltaTime * 0.5f);

        if (timer >= eatingTime || _DataAgent.Energy.value >= 0.95f)
        {
            if (_DataAgent.WC.value < 0.2f)
            {
                _StateMachine.ChangeState(TypeState.Banno);
                MoveToNewLocation(Location.Banno);
            }
            else if (_DataAgent.Sleep.value < 0.3f && !_DataAgent.IsInBathroom)
            {
                _StateMachine.ChangeState(TypeState.Dormir);
                MoveToNewLocation(Location.Dormitorio);
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
        // Limpiar o finalizar animaciones de comer
    }

    private void MoveToNewLocation(Location newLocation)
    {
        var route = PathMap.routes[(Location.Comedor, newLocation)];
        List<Transform> transforms = route.Select(loc => WaypointManager.Instance.GetWaypoint(loc)).ToList();

        // Iniciar movimiento hacia el nuevo destino
        _Movement.FollowPath(transforms);
    }
}

