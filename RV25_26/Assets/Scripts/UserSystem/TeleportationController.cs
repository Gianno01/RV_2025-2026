using UnityEngine;

public class TeleportationController : MonoBehaviour
{
    [SerializeField] private Transform[] spotPos;
    void Update()
    {
        HandleTeleport();
    }

    private void HandleTeleport()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            transform.position = new Vector3(spotPos[0].position.x, transform.position.y, spotPos[0].position.z);
        }else if (Input.GetKey(KeyCode.O))
        {
            transform.position = new Vector3(spotPos[1].position.x, transform.position.y, spotPos[1].position.z);
        }else if (Input.GetKey(KeyCode.P))
        {
            transform.position = new Vector3(spotPos[2].position.x, transform.position.y, spotPos[2].position.z);
        }
    }
}