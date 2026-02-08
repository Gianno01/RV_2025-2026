using UnityEngine;
using UnityEngine.UI; // Necessario per gestire l'immagine della UI

public class InteractionController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Distanza massima per interagire")]
    public float interactionDistance = 3f; // Ridotta per interazioni ravvicinate
    [Tooltip("Larghezza del fascio di interazione (raggio della sfera)")]
    public float interactionRadius = 0.2f; 
    [Tooltip("Layer degli oggetti interattivi")]
    public LayerMask interactionLayer;

    [Header("References")]
    public Transform playerCamera; 
    public GameObject tutorialPanel; 

    [Header("UI Pointer")]
    [Tooltip("Trascina qui l'immagine del puntatore UI")]
    public Image pointerImage; 
    public Color normalColor = Color.white;
    public Color interactColor = Color.green;

    [Header("Keys")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode tutorialKey = KeyCode.H;

    private IsInteractable _currentInteractable;

    private void Update()
    {
        HandleInteraction();
        HandleTutorial();
    }

    void HandleInteraction()
    {
        // Definiamo il raggio partendo dalla camera
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Utilizziamo SphereCast invece di Raycast per creare un "fascio" di volume
        // ray: origine e direzione
        // interactionRadius: quanto è largo il fascio
        // interactionDistance: quanto va lontano
        bool hitSomething = Physics.SphereCast(ray, interactionRadius, out hit, interactionDistance, interactionLayer);

        if (hitSomething)
        {
            // Usiamo GetComponentInParent per intercettare script su oggetti complessi (come gli NPC)
            IsInteractable interactable = hit.collider.GetComponentInParent<IsInteractable>();

            if (interactable != null)
            {
                // Cambia colore al puntatore UI
                UpdatePointerColor(interactColor);

                if (interactable != _currentInteractable)
                {
                    _currentInteractable?.OnLostFocus();
                    _currentInteractable = interactable;
                    _currentInteractable.OnFocus();
                }

                if (Input.GetKeyDown(interactKey))
                {
                    _currentInteractable.Interact();
                }
                return; 
            }
        }

        // Se non colpiamo nulla o l'oggetto non è interattivo
        ResetInteraction();
    }

    private void UpdatePointerColor(Color newColor)
    {
        if (pointerImage != null)
        {
            pointerImage.color = newColor;
        }
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
            if (tutorialPanel != null)
            {
                bool isActive = tutorialPanel.activeSelf;
                tutorialPanel.SetActive(!isActive);

                Cursor.lockState = !isActive ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = !isActive;
            }
        }
    }

    // Disegna il fascio nella Scene View per aiutarti a regolarlo
    private void OnDrawGizmosSelected()
    {
        if (playerCamera == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(playerCamera.position, playerCamera.forward * interactionDistance);
        Gizmos.DrawWireSphere(playerCamera.position + playerCamera.forward * interactionDistance, interactionRadius);
    }
}