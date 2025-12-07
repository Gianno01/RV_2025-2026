using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Distanza massima per interagire")]
    public float interactionDistance = 10f;
    [Tooltip("Layer degli oggetti interattivi (per non colpire il player stesso)")]
    public LayerMask interactionLayer;

    [Header("References")]
    public Transform playerCamera; // Trascina qui la Main Camera
    public GameObject tutorialPanel; // Trascina qui il pannello UI del tutorial

    [Header("Keys")]
    public KeyCode interactKey = KeyCode.Q;
    public KeyCode tutorialKey = KeyCode.H;

    private void Update()
    {
        // 1. GESTIONE INTERAZIONE (Tasto E)
        HandleInteraction();

        // 2. GESTIONE TUTORIAL (Tasto H)
        HandleTutorial();
    }

    void HandleInteraction()
    {
        // Crea un raggio che parte dalla camera e va in avanti
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Disegna una linea rossa nella scena (visibile solo in Scene view) per debug
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

        // Se il raggio colpisce qualcosa entro la distanza e nel layer giusto
        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            // Cerchiamo se l'oggetto colpito ha il componente IsInteractable
            IsInteractable interactable = hit.collider.GetComponent<IsInteractable>();

            if (interactable != null)
            {
                // Qui potresti mostrare una UI tipo "Premi E per interagire"
                Debug.Log("Posso interagire con: " + interactable.GetDescription());

                if (Input.GetKeyDown(interactKey))
                {
                    interactable.Interact();
                }
            }
        }
    }

    void HandleTutorial()
    {
        if (Input.GetKeyDown(tutorialKey))
        {
            if (tutorialPanel != null)
            {
                // Inverte lo stato attivo (se è acceso lo spegne, e viceversa)
                bool isActive = tutorialPanel.activeSelf;
                tutorialPanel.SetActive(!isActive);

                // Opzionale: Blocca/Sblocca il cursore se apri il tutorial
                if (!isActive) // Se lo stiamo aprendo
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else // Se lo stiamo chiudendo
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            else
            {
                Debug.LogWarning("Nessun pannello tutorial assegnato nell'Inspector!");
            }
        }
    }
}
