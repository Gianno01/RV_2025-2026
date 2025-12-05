using System;
using UnityEngine;

/// <summary>
/// Ogni volta che si necessita di un evento, creare un'istanza di AppEventData negli asset
/// o riutilizzare una delle esistenti si presta al caso. Esempio: Se voglio riprodurre l'hint audio della quest corrente,
/// creo un AppEventData OnHint. Entrambi tra QuestController e VoiceController hanno una reference a questo evento.
/// QuestController invocherà l'evento con OnHint.RaiseWithParam() per passare la clip audio da riprodurre.
/// Il parametro è di tipo object e occorrerà fare una conversione esplicita per utilizzarlo nella callback chiamata.
/// Nel caso servissero più parametri, usare una struct. Se non necessari, utilizzare OnHint.Raise().
/// VoiceController ascolterà l'evento associando una callback con OnHint.OnParamEvent += Callback, oppure OnHint.OnEvent += Callback.
/// Ricordarsi di dissociare le callback. 
/// </summary>

[CreateAssetMenu(fileName = "AppEventData", menuName = "Scriptable Objects/AppEventData")]
public class AppEventData : ScriptableObject
{
    public Action<object> OnParamEvent;
    public Action OnEvent;

    public void RaiseWithParam(object value)
    {
        OnParamEvent?.Invoke(value);
    }

    public void Raise()
    {
        OnEvent?.Invoke();
    }
}
