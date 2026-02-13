using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionProperty gripAction;
    public InputActionProperty moveAction;
    public InputActionProperty lookAction;
    public Transform leftHand;
    public Transform vrOrigin;
    public Vector3 halfBoxExtens;
    public float rayLength = 10f;
    private GameObject gripObj;
    public float rotSpeed;
    public float speed;

    Vector3 origin = Vector3.zero;
    Vector3 direction = Vector3.zero;
    public AppEventData appEventData;

    void Update()
    {
        handleOrientation();
        handleMotion();
        handleGrip();
    }

    private void handleOrientation()
    {
        Vector2 inputDir = lookAction.action.ReadValue<Vector2>();
        if (inputDir == Vector2.zero) return;
        Vector3 targetDir = new Vector3(inputDir.x, 0, 0);
        targetDir = targetDir.normalized;
        Vector3 rotDir = Camera.main.transform.TransformDirection(targetDir);
        float rotationStep = rotSpeed * Math.Abs(inputDir.x) * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(vrOrigin.forward, rotDir, rotationStep, 0);
        newDir.y = 0;
        vrOrigin.rotation = Quaternion.LookRotation(newDir);
    }

    private void handleMotion()
    {
        Vector2 inputDir = moveAction.action.ReadValue<Vector2>();
        if (inputDir == Vector2.zero) return;
        Vector3 targetDir = new Vector3(inputDir.x, 0, inputDir.y);
        targetDir = targetDir.normalized;
        Vector3 moveDir = Camera.main.transform.TransformDirection(targetDir);
        moveDir.y = 0;
        vrOrigin.Translate(moveDir * speed * Time.deltaTime, Space.World);
    }

    private void handleGrip()
    {
        float gripValue = gripAction.action.ReadValue<float>();
        if (gripValue > 0.8f)
        {
            if (!gripObj) throwRay();
        }
        else if(gripObj)
        {
            releaseObj();
        }
    }

    private void throwRay()
    {
        origin = Camera.main.transform.position;
        direction = Camera.main.transform.forward;

        // Lancia il raycast
        if (Physics.BoxCast(origin, halfBoxExtens, direction, out RaycastHit hit, Quaternion.identity, rayLength))
        {
            if (hit.collider.gameObject.tag != "Interactable") return;
            gripObj = hit.collider.gameObject;
            gripObj.GetComponent<Rigidbody>().isKinematic = true;
            gripObj.GetComponent<Collider>().enabled = false;
            gripObj.transform.position = leftHand.position;
            gripObj.transform.parent = leftHand;
        }
    }

    private void releaseObj()
    {
        gripObj.transform.parent = null;
        gripObj.GetComponent<Collider>().enabled = true;
        gripObj.GetComponent<Rigidbody>().isKinematic = false;
        gripObj = null;
    }
}
