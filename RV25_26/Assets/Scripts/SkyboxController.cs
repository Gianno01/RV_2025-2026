using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [SerializeField] private float _skyboxSpeed = 1f;
    [SerializeField] private float _sunSpeed = 1f;
    [SerializeField] private Transform _directionalLight;
    void Update() {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * _skyboxSpeed);
        _directionalLight.Rotate(Vector3.up, Time.deltaTime * _sunSpeed, Space.World);
        DynamicGI.UpdateEnvironment(); 
    }
}