using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrippableItem : MonoBehaviour, IsInteractable 
{
    [Header("Configurazione")]
    [SerializeField] private string _itemName = "Oggetto";
    [SerializeField] private Vector3 _followOffset = new Vector3(0.6f, -0.2f, 0.7f); 
    [SerializeField] private KeyCode _interactionKey = KeyCode.E; // Il tasto per interagire

    private Rigidbody _rb;
    private bool _isGrabbed = false;
    private Transform _playerTransform;

    void Start() 
    {
        _rb = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _playerTransform = player.transform;
    }

    // Chiamato dal Raycast del Player solo per il PRIMO contatto (Prendere)
    public void Interact() 
    {
        if (!_isGrabbed) Grab();
    }

    void Update()
    {
        // Se l'oggetto è in mano, controlla se premi il tasto per lasciarlo
        // Questo ignora il fatto che tu lo stia guardando o meno
        if (_isGrabbed && Input.GetKeyDown(_interactionKey))
        {
            Drop();
        }
    }

    private void Grab() 
    {
        if (_playerTransform == null) return;
        _isGrabbed = true;
        _rb.isKinematic = true; 
        _rb.useGravity = false;
    }

    private void Drop() 
    {
        _isGrabbed = false;
        _rb.isKinematic = false; 
        _rb.useGravity = true;
        // Opzionale: aggiunge una piccola spinta in avanti quando lo lasci
        _rb.AddForce(_playerTransform.forward * 2f, ForceMode.Impulse);
    }

    void LateUpdate() 
    {
        if (_isGrabbed && _playerTransform != null) 
        {
            Vector3 targetPosition = _playerTransform.position + 
                                     (_playerTransform.right * _followOffset.x) + 
                                     (_playerTransform.forward * _followOffset.z) +
                                     (_playerTransform.up * _followOffset.y);
            
            transform.position = targetPosition;
            transform.rotation = _playerTransform.rotation;
        }
    }

    public string GetDescription() 
    {
        return _isGrabbed ? $"Lascia {_itemName}" : $"Prendi {_itemName}";
    }
}