using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private AppEventData _onGateStart;
    [SerializeField] private AppEventData _onGateExit;
    [SerializeField] private AppEventData _onFadeInEnd;

    public void EnterGate()
    {
        this.enabled = true;
        _onFadeInEnd.OnEvent += HandleOnFadeInEnd;
    }

    public void ExitGate()
    {
        this.enabled = false;
    }

    public void StartTransition()
    {
        _onGateStart.Raise();
    }

    private void HandleOnFadeInEnd()
    {
        _onFadeInEnd.OnEvent -= HandleOnFadeInEnd;
        _onGateExit.Raise();
    }
}