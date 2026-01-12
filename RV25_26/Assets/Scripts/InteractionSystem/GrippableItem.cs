using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrippableItem : MonoBehaviour, IsInteractable 
{
    [Header("Configurazione")]
    [SerializeField] private string _itemName = "Oggetto";
    [SerializeField] private Vector3 _followOffset = new Vector3(0.6f, -0.2f, 0.7f); 
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;

    private Rigidbody _rb;
    private Collider _collider; 
    private bool _isGrabbed = false;
    private bool _canDrop = false; // Variabile di sicurezza
    private Transform _playerTransform;

    void Start() 
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>(); 
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _playerTransform = player.transform;
    }

    public void Interact() 
    {
        // Se non è preso, lo prendiamo
        if (!_isGrabbed) Grab();
    }

    void Update()
    {
        if (_isGrabbed)
        {
            // Controlliamo il rilascio solo se è passato almeno un frame dalla presa
            if (Input.GetKeyDown(_interactionKey) && _canDrop)
            {
                Drop();
            }

            // Dopo il primo frame di Grab, permettiamo il Drop
            _canDrop = true;
        }
    }

    private void Grab() 
    {
        if (_playerTransform == null) return;
        
        _isGrabbed = true;
        _canDrop = false; // Impediamo il rilascio immediato in questo frame
        
        _rb.isKinematic = true; 
        _rb.useGravity = false;

        if (_collider != null) _collider.enabled = false; 
    }

    private void Drop() 
    {
        _isGrabbed = false;
        _canDrop = false;
        
        _rb.isKinematic = false; 
        _rb.useGravity = true;

        if (_collider != null) _collider.enabled = true;

        _rb.AddForce(_playerTransform.forward * 2f, ForceMode.Impulse);
    }

    void LateUpdate() 
    {
        if (_isGrabbed && _playerTransform != null) 
        {
            Vector3 targetPosition = _playerTransform.TransformPoint(_followOffset);
            transform.position = targetPosition;
            transform.rotation = _playerTransform.rotation;
        }
    }

    public string GetDescription() => _isGrabbed ? $"Lascia {_itemName}" : $"Prendi {_itemName}";
    public void OnFocus() { }
    public void OnLostFocus() { }
}