using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] private QuestData[] _questDatas;
    [SerializeField] private AppEventData OnStartQuest;
    private int _currentQuestIndex;

    void Start()
    {
        OnStartQuest.OnParamEvent += HandleOnStartQuest;
    }

    void OnDisable()
    {
        OnStartQuest.OnParamEvent -= HandleOnStartQuest;
    }

    private void HandleOnStartQuest(object param)
    {
        _currentQuestIndex = (int) param;
        //fai partire l'hint dopo sectowait
        //aggiungere trigger collider che scatenano l'hint
        /*DOVirtual.Float(0f, 100f, 2f, value =>
        {
            DoVirtual._questDatas[_currentQuestIndex].SecToWait;
        });*/
    }
}