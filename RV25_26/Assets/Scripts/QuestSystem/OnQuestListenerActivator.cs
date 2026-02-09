using System.Linq;
using UnityEngine;
public class OnQuestListenerActivator : OnQuestListener
{
    [SerializeField] private int[] _startQuests;
    [SerializeField] private int[] _endQuests;

    [SerializeField] private bool _isNotInTheScene;

    [Header("Se l'activator si applica ad un gameobject, compila questa sezione")]
    [SerializeField] private GameObject _gameObject;
    [Header("Se il gameobject non è nella scena, compila questa sezione")]
    [SerializeField] private string _gameObjectName;

    [Header("Se l'activator si applica ad un singolo componente, compila questa sezione")]
    [SerializeField] private bool _effectOnOneComponent;
    [SerializeField] private MonoBehaviour _component;
    [Header("Se il componente non è nella scena, compila questa sezione")]
    [SerializeField] private string _componentName;

    [SerializeField] private AppEventData _onActivate;
    [SerializeField] private AppEventData _onDeactivate;
    private bool init = false;
    private MonoBehaviour _obj;
    private GameObject _gObj;

    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;
        if (_startQuests.Contains(questIndex))
        {
            SetActivation(true);
            if(_onActivate != null) _onActivate.Raise();
        }
        else if (_endQuests.Contains(questIndex))
        {
            if(_onDeactivate != null){_onDeactivate.Raise();}
            SetActivation(false);
        }
    }

    private void Init()
    {
        if (_effectOnOneComponent)
        {
            if(_component != null)
            {
                _obj = _component;
            }else if (_isNotInTheScene && _gameObjectName != null && _componentName != null)
            {
                _obj = (MonoBehaviour) GameObject.Find(_gameObjectName).GetComponent(_componentName);
            }
        }
        else
        {
            if(_gameObject != null)
            {
                _gObj = _gameObject;
            }
            else if(_isNotInTheScene && _gameObjectName != null)
            {
                _gObj = GameObject.Find(_gameObjectName);
            }
        }
        init = true;
    }

    private void SetActivation(bool active)
    {
        if(!init) Init();

        if (_effectOnOneComponent)
        {
            _obj.enabled = active;
        }
        else
        {
            _gObj.SetActive(active);
        }
    }
}