using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormir : Humano
{
    private float sleepRecoveryRate = 0.1f; // Tasa de recuperaci�n de sue�o por segundo
    private float energyRecoveryRate = 0.05f; // Tasa de recuperaci�n de energ�a por segundo

    private void Awake()
    {
        typestate = TypeState.Dormir;
        LocadComponent();
    }

    public override void Enter()
    {
        _DataAgent.IsSleeping = true;
        // Iniciar animaci�n de dormir
    }

    public override void Execute()
    {
        // Recuperar sue�o y energ�a
        _DataAgent.Sleep.value = Mathf.Min(1f, _DataAgent.Sleep.value + sleepRecoveryRate * Time.deltaTime);
        _DataAgent.Energy.value = Mathf.Min(1f, _DataAgent.Energy.value + energyRecoveryRate * Time.deltaTime);

        // Si ya descans� lo suficiente
        if (_DataAgent.Sleep.value >= 0.95f)
        {
            // Verificar otras necesidades
            if (_DataAgent.WC.value < 0.2f)
            {
                _StateMachine.ChangeState(TypeState.Banno);
            }
            else if (_DataAgent.Energy.value < 0.5f)
            {
                _StateMachine.ChangeState(TypeState.Comer);
            }
            else
            {
                _StateMachine.ChangeState(TypeState.Jugar);
            }
        }

        base.Execute();
    }

    public override void Exit()
    {
        _DataAgent.IsSleeping = false;
        // Finalizar animaci�n de dormir
    }
}
