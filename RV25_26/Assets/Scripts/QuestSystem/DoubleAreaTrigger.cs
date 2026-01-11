using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// AreaTrigger è pensato per invocare una callback al momento della collisione tra un AreaTrigger e un oggetto del layer scelto 
/// La callback è da specificare via inspector, specificando GameObject, Componente e metodo.
/// </summary>

[RequireComponent(typeof(Collider))]
public class DoubleAreaTrigger : AreaTrigger
{
    [SerializeField] private AreaTrigger _otherAreaTrigger;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(!this.enabled) return;
        
        _otherAreaTrigger.enabled = !_otherAreaTrigger.enabled;
        this.enabled = !this.enabled;
    }
}