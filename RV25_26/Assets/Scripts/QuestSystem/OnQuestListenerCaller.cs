using System;
using System.Reflection;
using DG.Tweening;
using UnityEngine;

public class OnQuestListenerCaller : OnQuestListener
{
    [SerializeField] private bool _isNotInTheScene;

    [SerializeField] private int _questIndex;
    [SerializeField] private float _secToWaitBeforeCall;
    [SerializeField] private MonoBehaviour _component;
    [SerializeField] private string _methodName;
    [Tooltip("Ammesso un unico parametro intero")]
    [SerializeField] private int _param;

    [Header("Se il componente non è nella scena, compila questa sezione")]
    [SerializeField] private string _gameObjectName;
    [SerializeField] private string _componentName;
    
    private MonoBehaviour _obj;
    private MethodInfo _method;
    private int _paramNumber;
    private bool init = false;

    void Init()
    {
        if(_component != null && _methodName != null)
        {
            _obj = _component;
        }
        else if(_isNotInTheScene && _gameObjectName != null && _componentName != null && _methodName != null)
        {
            _obj = (MonoBehaviour) GameObject.Find(_gameObjectName).GetComponent(_componentName);
        }

        Type type = _obj.GetType();
        _method = type.GetMethod(_methodName);
        _paramNumber = _method.GetParameters().Length;

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

        if(_secToWaitBeforeCall != 0) DOVirtual.DelayedCall(_secToWaitBeforeCall, CallNow);
        else CallNow();
    }

    private void CallNow()
    {
        if(_paramNumber == 1) _method.Invoke(_obj, new object[]{_param});
        else if(_paramNumber == 0) _method.Invoke(_obj, new object[]{});
        else{Debug.LogWarning("IL METODO DA INVOCARE HA PIù DI UN PARAMETRO. NON SUPPORTATO");}
    }
}