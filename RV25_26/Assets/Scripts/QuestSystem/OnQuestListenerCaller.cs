using System;
using System.Reflection;
using UnityEngine;

public class OnQuestListenerCaller : OnQuestListener
{
    [SerializeField] private int _questIndex;
    [SerializeField] private bool _isReferenceToOtherGameobject;
    [SerializeField] private string _gameObjectName;
    [SerializeField] private string _componentName;
    [SerializeField] private string _methodName;
    [Tooltip("Ammesso un unico parametro intero")]
    [SerializeField] private int _param;
    
    private MonoBehaviour _obj;
    private MethodInfo _method;
    private int _paramNumber;
    private bool init = false;

    void Init()
    {
        if(_componentName != null && _methodName != null)
        {
            Type type = Type.GetType(_componentName);
            _method = type.GetMethod(_methodName);
            _paramNumber = _method.GetParameters().Length;

            if (_isReferenceToOtherGameobject || _gameObjectName != null)
            {
                _obj = (MonoBehaviour) GameObject.Find(_gameObjectName).GetComponent(_componentName);
            }
            else
            {
                _obj = (MonoBehaviour) gameObject.GetComponent(_componentName);
            }
        }
        init = true;
    }

    protected override void HandleOnQuestChange(object param)
    {
        if(!init) Init();

        int quest = (int) param;
        if(_questIndex == quest)
        {
            Call();
        }
    }

    private void Call()
    {
        if(!this.enabled || _method == null || _obj == null) return;

        if(_paramNumber == 1) _method.Invoke(_obj, new object[]{_param});
        else if(_paramNumber == 0) _method.Invoke(_obj, new object[]{});
        else{Debug.LogWarning("IL METODO DA INVOCARE HA PIù DI UN PARAMETRO. NON SUPPORTATO");}
    }
}