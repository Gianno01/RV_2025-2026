using UnityEngine;

public class ItemReceiver : MonoBehaviour, IsInteractable
{
    [Header("Consegna")]
    public string receiverName = "receiver";
    public bool destroyOnDelivery = false; // Scompare?
    public Transform deliveryPoint;        // O si piazza qui?

    [Header("Eventi (Opzionale)")]
    [Tooltip("Trascina qui l'evento se vuoi far avanzare la quest, altrimenti lascia vuoto.")]
    [SerializeField] private AppEventData _onDeliveryObject;
    private Outline _outline;

    void Awake() 
    {
        _outline = GetComponent<Outline>();
        if (_outline != null) _outline.enabled = false;
    }

    public void ReceiveItem(GrippableItem item)
    {
        Debug.Log("GrippableItem: " + item);
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

        // 3. INVIO EVENTO (Solo se assegnato nell'Inspector)
        if (_onDeliveryObject != null)
        {
            _onDeliveryObject.Raise();
            Debug.Log($"Evento quest lanciato da {gameObject.name}.");
        }
        else
             Debug.Log($"nessun evento associato alla consegna.");    
        // Se l'oggetto è stato consegnato, togliamo il focus per resettare l'outline
        OnLostFocus();
    }

    public void Interact() => Debug.Log("Interazione con " + receiverName);
    public string GetDescription() => "Consegna a " + receiverName;
    public void OnFocus() { if (_outline != null) _outline.enabled = true; }
    public void OnLostFocus() { if (_outline != null) _outline.enabled = false; }
}