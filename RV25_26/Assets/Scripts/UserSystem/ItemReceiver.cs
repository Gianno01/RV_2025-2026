using UnityEngine;

public class ItemReceiver : MonoBehaviour, IsInteractable
{
    [Header("Consegna")]
    public string receiverName = "Tavolo";
    public bool destroyOnDelivery = false; // Scompare?
    public Transform deliveryPoint;        // O si piazza qui?

    private Outline _outline;

    void Awake() 
    {
        _outline = GetComponent<Outline>();
        if (_outline != null) _outline.enabled = false;
    }

    public void ReceiveItem(GrippableItem item)
    {
        if (item == null) return;

        if (destroyOnDelivery)
        {
            Destroy(item.gameObject);
            Debug.Log(item.name + " eliminato.");
        }
        else if (deliveryPoint != null)
        {
            item.PlaceOnTarget(deliveryPoint);
            Debug.Log(item.name + " piazzato su " + receiverName);
        }
        
        // Se l'oggetto è stato consegnato, togliamo il focus per resettare l'outline
        OnLostFocus();
    }

    public void Interact() => Debug.Log("Interazione con " + receiverName);
    public string GetDescription() => "Consegna a " + receiverName;
    public void OnFocus() { if (_outline != null) _outline.enabled = true; }
    public void OnLostFocus() { if (_outline != null) _outline.enabled = false; }
}