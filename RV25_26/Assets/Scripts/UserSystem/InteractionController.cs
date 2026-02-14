using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [Header("Settings")]
    public float interactionDistance = 3f;
    public float interactionRadius = 0.2f; 
    public LayerMask interactionLayer;

    [Header("References")]
    //public Transform playerCamera; 
    public Camera playerCamera;
    public GameObject tutorialPanel; 

    [Header("UI Pointer")]
    public Image pointerImage; 
    public Color normalColor = Color.white;
    public Color interactColor = Color.green;

    [Header("Keys")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode tutorialKey = KeyCode.H;
    public KeyCode homeKey = KeyCode.U;

    [Header("Events")]
    [SerializeField] private AppEventData _onHelpRequest;
    [SerializeField] private AppEventData _onHomeSceneRequest;

    private IsInteractable _currentInteractable;
    private Vector3 _screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

    private void Update()
    {
        HandleInteraction();
        HandleTutorial();
        HandleHome();
    }

    void HandleInteraction()
    {
        //Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        Ray ray = playerCamera.ScreenPointToRay(_screenCenter);
        RaycastHit hit;

        bool hitSomething = Physics.Raycast(ray, out hit, interactionDistance, interactionLayer);

        if (hitSomething)
        {
            IsInteractable interactable = hit.collider.GetComponentInParent<IsInteractable>();

            if (interactable != null)
            {
                UpdatePointerColor(interactColor);

                if (interactable != _currentInteractable)
                {
                    _currentInteractable?.OnLostFocus();
                    _currentInteractable = interactable;
                    _currentInteractable.OnFocus();
                }

                if (Input.GetKeyDown(interactKey))
                {
                    // SE abbiamo un oggetto in mano, proviamo a consegnarlo
                    if (GrippableItem.HeldItem != null)
                    {
                        _currentInteractable.ReceiveItem(GrippableItem.HeldItem);
                    }
                    else
                    {
                        _currentInteractable.Interact();
                    }
                }
                return; 
            }
        }

        // Se non guardiamo nulla e premiamo il tasto, lasciamo cadere l'oggetto
        if (Input.GetKeyDown(interactKey) && GrippableItem.HeldItem != null)
        {
            GrippableItem.HeldItem.Drop();
        }

        ResetInteraction();
    }

    private void UpdatePointerColor(Color newColor)
    {
        if (pointerImage != null) pointerImage.color = newColor;
    }

    private void ResetInteraction()
    {
        UpdatePointerColor(normalColor);
        if (_currentInteractable != null)
        {
            _currentInteractable.OnLostFocus();
            _currentInteractable = null;
        }
    }

    void HandleTutorial()
    {
        if (Input.GetKeyDown(tutorialKey))
        {
            _onHelpRequest.Raise();
        }
        /*if (Input.GetKeyDown(tutorialKey) && tutorialPanel != null)
        {
            bool isActive = tutorialPanel.activeSelf;
            tutorialPanel.SetActive(!isActive);
            Cursor.lockState = !isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !isActive;
        }*/
    }

    private void HandleHome()
    {
        if (Input.GetKeyDown(homeKey))
        {
            FromSceneToScene fromSceneToScene;
            fromSceneToScene.from = "MasterScene";
            fromSceneToScene.to = "HomeScene";
            _onHomeSceneRequest.RaiseWithParam(fromSceneToScene);
        }
    }
}