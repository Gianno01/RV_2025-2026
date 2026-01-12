using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Distanza massima per interagire")]
    public float interactionDistance = 5f;
    [Tooltip("Layer degli oggetti interattivi")]
    public LayerMask interactionLayer;

    [Header("References")]
    public Transform playerCamera; 
    public GameObject tutorialPanel; 

    [Header("Keys")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode tutorialKey = KeyCode.H;

    // Riferimento all'oggetto che stiamo guardando attualmente
    private IsInteractable _currentInteractable;

    private void Update()
    {
        // 1. GESTIONE INTERAZIONE E FEEDBACK VISIVO
        HandleInteraction();

        // 2. GESTIONE TUTORIAL
        HandleTutorial();
    }

    void HandleInteraction()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debug visivo del raggio nella scena
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            IsInteractable interactable = hit.collider.GetComponent<IsInteractable>();

            if (interactable != null)
            {
                // Se l'oggetto che stiamo guardando è DIVERSO da quello del frame precedente
                if (interactable != _currentInteractable)
                {
                    // Se stavamo guardando qualcos'altro prima, spegniamo il suo feedback
                    _currentInteractable?.OnLostFocus();

                    // Aggiorniamo l'oggetto corrente e attiviamo il suo feedback
                    _currentInteractable = interactable;
                    _currentInteractable.OnFocus();
                    
                    Debug.Log("Focus su: " + _currentInteractable.GetDescription());
                }

                // Esegui l'interazione se viene premuto il tasto
                if (Input.GetKeyDown(interactKey))
                {
                    _currentInteractable.Interact();
                }

                return; // Esce dalla funzione per non resettare il focus
            }
        }

        // Se il raggio non colpisce nulla o non colpisce un oggetto interattivo
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

                if (!isActive)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }
    }
}