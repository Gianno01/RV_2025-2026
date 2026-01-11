using UnityEngine;
public class TestItem : MonoBehaviour, IsInteractable 
{
    public void Interact() {
        Debug.Log("Ho interagito con l'oggetto!");
        // Qui metterai il codice per raccogliere/aprire
        Destroy(gameObject); // Esempio: distrugge l'oggetto
    }
    public string GetDescription() { return "Oggetto di prova"; }
}