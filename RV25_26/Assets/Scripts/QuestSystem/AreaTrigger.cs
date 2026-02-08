using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// AreaTrigger è pensato per invocare una callback al momento della collisione tra un AreaTrigger e un oggetto del layer scelto 
/// La callback è da specificare via inspector, specificando GameObject, Componente e metodo.
/// </summary>

[RequireComponent(typeof(Collider))]
public class AreaTrigger : MonoBehaviour
{
    [SerializeField] private bool _isReferenceToOtherGameobject;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private string _gameObjectName;
    [SerializeField] private string _componentName;
    [SerializeField] private string _methodName;
    [Tooltip("Ammesso un unico parametro intero")]
    [SerializeField] private int _param;
    [SerializeField] private LayerMask _collisionLayer;
    private MonoBehaviour _obj;
    private MethodInfo _method;
    private int _paramNumber;
    private bool init = false;

    private void Init()
    {
        Type type = Type.GetType(_componentName);
        _method = type.GetMethod(_methodName);
        _paramNumber = _method.GetParameters().Length;

        if (_isReferenceToOtherGameobject)
        {
            if(_gameObject == null) _gameObject = GameObject.Find(_gameObjectName);
            _obj = (MonoBehaviour) _gameObject.GetComponent(_componentName);
        }
        else
        {
            _obj = (MonoBehaviour) gameObject.GetComponent(_componentName);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(!this.enabled) return;
        
        if(!init) Init();

        if(((1 << other.gameObject.layer) & _collisionLayer.value) != 0)
        {
            if(_paramNumber == 1) _method.Invoke(_obj, new object[]{_param});
            else if(_paramNumber == 0) _method.Invoke(_obj, new object[]{});
            else{Debug.LogWarning("IL METODO DA INVOCARE HA PIù DI UN PARAMETRO. NON SUPPORTATO");}
        }
    }
}