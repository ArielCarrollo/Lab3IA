using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugar : Humano
{
    private void Awake()
    {
        typestate = TypeState.Jugar;
        LocadComponent();
    }

    public override void Execute()
    {
        // Consumir energ�a
        _DataAgent.DiscountEnergy();

        // Consumir sue�o m�s r�pido al jugar (solo si no est� en el ba�o)
        if (!_DataAgent.IsInBathroom)
        {
            _DataAgent.Sleep.value = Mathf.Max(0, _DataAgent.Sleep.value - Time.deltaTime * 0.02f);
        }

        // Consumir necesidad de ba�o m�s r�pido al jugar
        _DataAgent.WC.value = Mathf.Max(0, _DataAgent.WC.value - Time.deltaTime * 0.03f);

        // Verificar prioridades
        if (_DataAgent.WC.value < 0.15f)
        {
            _StateMachine.ChangeState(TypeState.Banno);
        }
        else if (_DataAgent.Energy.value < 0.25f)
        {
            _StateMachine.ChangeState(TypeState.Comer);
        }
        else if (_DataAgent.Sleep.value < 0.2f && !_DataAgent.IsInBathroom)
        {
            // Solo verificar sue�o si no est� en el ba�o
            _StateMachine.ChangeState(TypeState.Dormir);
        }

        base.Execute();
    }
}
