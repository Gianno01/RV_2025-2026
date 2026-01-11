using DG.Tweening;
using UnityEngine;

/// <summary>
/// QuestController raggruppa la sequenza delle quest con la lista _questDatas. Reagisce all'evento _onStartQuest,
/// definendo la nuova quest corrente in base all'indice passato con l'evento. L'hint della quest viene eseguito dopo
/// tot secondi di tempo in base al valore di SecToWait della QuestData. Se -1, allora ci si aspetta che l'hint venga
/// eseguito come effetto di altre cause (vedi AreaTrigger). Si esegue l'hint con il metodo PlayHint(). Eseguire l'hint
/// vuol dire invocare l'evento _onHint. Un Hint corrisponde ad una clip audio che gestisce VoiceController.
/// Una quest si completa interagendo con un Item o un Character. Questi invocheranno l'evento _onStartQuest
/// per passare alla quest successiva. _onStartQuest viene invocato solo se la quest corrente corrisponde a quella attesa
/// dall'Item o Character in quel momento per progredire. Se così non fosse, modellare il comportamento alternativo dell'item o character.
/// L'Item o Character conoscono l'index della quest corrente perché vengono aggiornati ad ogni suo cambiamento dall'evento _OnCurrentQuestChange
/// e si tengono una loro copia dell'index (Così si riduce l'accoppiamento tra classi).
/// </summary>
public class QuestController : MonoBehaviour
{
    [SerializeField] private QuestData[] _questDatas;
    [SerializeField] private AppEventData _onStartQuest;
    [SerializeField] private AppEventData _onHint;
    [SerializeField] private AppEventData _onCurrentQuestChange;
    [HideInInspector] public int _currentQuestIndex {get; private set;}

    void Start()
    {
        _onStartQuest.OnParamEvent += HandleOnStartQuest;
        // commenta se non vuoi testare il quest system nella scena QuestSystem
        //_onStartQuest.RaiseWithParam(1);
        //DOVirtual.DelayedCall(10, () => _onStartQuest.RaiseWithParam(0),false);
    }

    void OnDisable()
    {
        _onStartQuest.OnParamEvent -= HandleOnStartQuest;
    }

    private void HandleOnStartQuest(object param)
    {
        _currentQuestIndex = (int) param;
        _onCurrentQuestChange.RaiseWithParam(_currentQuestIndex);

        QuestData questData = _questDatas[_currentQuestIndex];
        if(questData.SecToWait != -1)
        {
            DOVirtual.DelayedCall(questData.SecToWait, PlayHint, false);
        }
    }

    public void PlayHint()
    {
        _onHint.RaiseWithParam(_questDatas[_currentQuestIndex].hintAudio);
    }
}