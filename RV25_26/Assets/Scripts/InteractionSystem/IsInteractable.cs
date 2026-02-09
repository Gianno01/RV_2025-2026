using UnityEngine;

public interface IsInteractable
{
    void Interact();
    string GetDescription();
    void OnFocus();     // Chiamato quando guardi l'oggetto
    void OnLostFocus(); // Chiamato quando distogli lo sguardo
    void ReceiveItem(GrippableItem item); //metodo consegna
}