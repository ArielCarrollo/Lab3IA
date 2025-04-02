using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _DataAgent.IsInBathroom = true; // Marcamos que est� en el ba�o
        // Iniciar animaci�n de ir al ba�o
    }

    public override void Execute()
    {
        timer += Time.deltaTime;

        // Aliviar la necesidad gradualmente
        _DataAgent.WC.value = Mathf.Lerp(_DataAgent.WC.value, 1f, Time.deltaTime * 2f);

        // Si ha terminado o ya no necesita
        if (timer >= bathroomTime || _DataAgent.WC.value >= 0.95f)
        {
            _DataAgent.IsInBathroom = false; // Ya no est� en el ba�o

            // Verificar otras necesidades (excepto sue�o, porque acaba de estar en el ba�o)
            if (_DataAgent.Energy.value < 0.3f)
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
        _DataAgent.IsInBathroom = false; // Por si acaso se sale de otro modo
        // Finalizar animaci�n de ba�o
    }
}

