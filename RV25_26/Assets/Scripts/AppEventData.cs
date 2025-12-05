using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AppEventData", menuName = "Scriptable Objects/AppEventData")]
public class AppEventData : ScriptableObject
{
    /// <summary>
    /// Delegato da associare a callback. Ricorda di dissociarle anche.
    /// </summary>
    public Action<object> OnParamEvent;
    public Action OnEvent;

    /// <summary>
    /// Invoca l'evento passando un parametro generico. In base all'istanza dello scriptable object, 
    /// potrai distinguere il parametro nella callback e farne una conversione esplicita.
    /// </summary>
    public void RaiseWithParam(object value)
    {
        OnParamEvent?.Invoke(value);
    }

    public void Raise()
    {
        OnEvent?.Invoke();
    }
}
