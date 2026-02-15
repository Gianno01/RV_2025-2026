using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrippableItem : MonoBehaviour, IsInteractable 
{
    // Variabile statica per sapere cosa stiamo portando
    public static GrippableItem HeldItem;

    [Header("Configurazione")]
    [SerializeField] private string _itemName = "Oggetto";
    [SerializeField] private Vector3 _followOffset = new Vector3(0.6f, -0.2f, 0.7f); 
    [SerializeField] private Vector3 _rotOffset = Vector3.zero; 
    [SerializeField] private AppEventData _onGripped;

    private Rigidbody _rb;
    private Collider _collider; 
    private bool _isGrabbed = false;
    private Transform _playerTransform;
    private Outline _outline;

    void Awake() 
    {
        _outline = GetComponent<Outline>();
        if (_outline != null) _outline.enabled = false;
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>(); 
    }

    void Start() 
    {
        GameObject playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (playerCamera != null) _playerTransform = playerCamera.transform;
    }

    public void Interact() 
    {
        if (!_isGrabbed && HeldItem == null) Grab();
    }

    private void Grab() 
    {
        if (_playerTransform == null) return;
        _isGrabbed = true;
        HeldItem = this;
        _rb.isKinematic = true; 
        _rb.useGravity = false;
        if (_collider != null) _collider.enabled = false; 
        if(_onGripped != null) _onGripped.Raise();
    }

    public void Drop() 
    {
        _isGrabbed = false;
        HeldItem = null;
        _rb.isKinematic = false; 
        _rb.useGravity = true;
        if (_collider != null) _collider.enabled = true;
        _rb.AddForce(_playerTransform.forward * 2f, ForceMode.Impulse);
    }

    public void PlaceOnTarget(Transform targetPoint)
    {
        _isGrabbed = false;
        HeldItem = null;
        _rb.isKinematic = true;
        _rb.useGravity = false;
        if (_collider != null) _collider.enabled = true;

        transform.SetParent(targetPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void LateUpdate() 
    {
        if (_isGrabbed && _playerTransform != null) 
        {
            transform.position = _playerTransform.TransformPoint(_followOffset);
            transform.rotation = _playerTransform.rotation * Quaternion.Euler(_rotOffset);
        }
    }

    public string GetDescription() => _isGrabbed ? $"Lascia {_itemName}" : $"Prendi {_itemName}";
    public void OnFocus() { if (_outline != null) _outline.enabled = true; }
    public void OnLostFocus() { if (_outline != null) _outline.enabled = false; }
    public void ReceiveItem(GrippableItem item) { /* Un oggetto non riceve un altro oggetto */ }
}