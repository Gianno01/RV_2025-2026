using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MotionController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Trascina qui la Main Camera che è figlia del Player")]
    public Transform playerCamera;

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float lookXLimit = 85f;

    [Header("Camera View Settings")]
    public KeyCode changeViewKey = KeyCode.V; // Tasto per cambiare visuale
    public Vector3 firstPersonPos = new Vector3(0, 0.8f, 0); // Posizione locale testa
    public Vector3 thirdPersonPos = new Vector3(0.5f, 1.5f, -3f); // Posizione dietro le spalle

    public AudioSpot audioSpot;

    // Stato interno
    private CharacterController characterController;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isFirstPerson = true;
    private Vector3 _previousMove = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Blocca il cursore al centro dello schermo e lo nasconde
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Imposta la visuale iniziale
        UpdateCameraPosition();
    }

    void Update()
    {
        // 1. Gestione Movimento (WASD)
        HandleMovement();

        // 2. Gestione Rotazione (Mouse)
        HandleRotation();

        // 3. Cambio Visuale
        if (Input.GetKeyDown(changeViewKey))
        {
            isFirstPerson = !isFirstPerson;
            UpdateCameraPosition();
        }
    }

    void HandleMovement()
    {
        // Controlla se siamo a terra per resettare la gravità accumulata
        bool isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Valore piccolo per tenerlo incollato al suolo
        }

        // Input WASD
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calcola la direzione relativa a dove guarda il player
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Gestione corsa (Shift sinistro)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        if(currentSpeed == sprintSpeed)
        {
            audioSpot.ChangePitch(1.5f);
        }
        else
        {
            audioSpot.ChangePitch(1);
        }
        
        if(move != Vector3.zero && _previousMove == Vector3.zero) audioSpot.PlayAudio();
        else if(move == Vector3.zero && _previousMove != Vector3.zero) audioSpot.StopAudio();

        _previousMove = move;

        // Muovi il Character Controller
        characterController.Move(move * currentSpeed * Time.deltaTime);

        // Salto (Opzionale ma standard)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Applica Gravità
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotazione Verticale (Camera) - Inverte la Y per non avere controlli "aereo"
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -lookXLimit, lookXLimit);

        // Applica rotazione alla telecamera (su/giù)
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotazione Orizzontale (Corpo del Player) - Ruota tutto il personaggio
        transform.Rotate(Vector3.up * mouseX);
    }

    void UpdateCameraPosition()
    {
        if (playerCamera == null) return;

        if (isFirstPerson)
        {
            // Prima Persona: La camera è dentro la "testa"
            playerCamera.localPosition = firstPersonPos;
        }
        else
        {
            // Terza Persona: La camera è dietro e in alto
            playerCamera.localPosition = thirdPersonPos;
        }
    }
}