using UnityEngine;

// Questa interfaccia è il contratto comune per tutto ciò che è interattivo
public interface IsInteractable
{
    void Interact();
    string GetDescription(); // Utile per mostrare "Premi E per aprire"
}