using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        timer += Time.deltaTime;

        // Recuperar energía mientras come
        _DataAgent.Energy.value = Mathf.Lerp(_DataAgent.Energy.value, 1f, Time.deltaTime * 0.5f);

        // Si ha comido suficiente tiempo o está lleno
        if (timer >= eatingTime || _DataAgent.Energy.value >= 0.95f)
        {
            // Verificar otras necesidades (WC primero)
            if (_DataAgent.WC.value < 0.2f)
            {
                _StateMachine.ChangeState(TypeState.Banno);
            }
            else if (_DataAgent.Sleep.value < 0.3f && !_DataAgent.IsInBathroom)
            {
                // Solo dormir si no está en el baño (aunque no debería estar aquí)
                _StateMachine.ChangeState(TypeState.Dormir);
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
        // Limpiar o finalizar animaciones de comer
    }
}
