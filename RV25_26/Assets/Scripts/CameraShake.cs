using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float magnitude;
    private Vector3 originalPos;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    private float dampingSpeed = 1.0f;
    private bool shaked = true;

    void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else if(!shaked)
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
            shaked = true;
        }
    }

    public void Shake()
    {
        originalPos = transform.localPosition;
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shaked = false;
    }
}