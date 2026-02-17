using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System; 

public enum NpcState { Idle, Patrol, Talk, Follow }

[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(BoxCollider))] 
[RequireComponent(typeof(Animator))] // Assicura che ci sia un Animator
public class NpcBrain : MonoBehaviour, IsInteractable, IStateChangeable
{
    [Header("Stato")]
    public NpcState currentState = NpcState.Idle;
    private TalkingCharacter _talkingVoice; 

    [Header("Inseguimento Temporaneo")]
    [Tooltip("Distanza alla quale l'NPC si ferma per iniziare a parlare")]
    public float talkDistance = 2.0f;

    [Header("Pattugliamento")]
    public Transform[] waypoints;
    public float waitTimeAtPoint = 2f;
    private int currentWaypointIndex = 0;

    [Header("Configurazione")]
    public float lookSpeed = 5f; 
    [Header("Info Ui")]
    public string npcName = "Dottore";
    public string actionVerb = "Parla con";

    private NavMeshAgent agent;
    private Transform playerTransform;
    private Outline _outline;
    private Animator _animator; // Riferimento all'Animator

    void Awake()
    {
        _outline = GetComponent<Outline>();
        if (_outline != null) _outline.enabled = false;
        
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>(); // Inizializzazione Animator

        foreach(Transform t in waypoints)
        {
            t.position = new Vector3(t.position.x, transform.position.y, t.position.z);
        }
    }

    void Start()
    {
        _talkingVoice = GetComponent<TalkingCharacter>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
        
        agent.updateRotation = true;

        if (waypoints.Length > 0 && currentState == NpcState.Idle)
        {
            ChangeState("Patrol");
        }
    }

    public void ChangeState(string state)
    {
        if (Enum.TryParse(state, true, out NpcState newState))
        {
            StopAllCoroutines();
            currentState = newState;

            switch (currentState)
            {
                case NpcState.Patrol:
                    agent.isStopped = false;
                    agent.stoppingDistance = 0.5f;
                    MoveToNextWaypoint();
                    UpdateAnimation(true); // Cammina
                    break;
                case NpcState.Follow:
                    agent.isStopped = false;
                    agent.stoppingDistance = talkDistance; 
                    UpdateAnimation(true); // Cammina
                    break;
                case NpcState.Idle:
                case NpcState.Talk:
                    agent.isStopped = true;
                    UpdateAnimation(false); // Fermo
                    break;
            }
        }
    }

    // Metodo helper per gestire il parametro "walk" dell'animator
    private void UpdateAnimation(bool isWalking)
    {
        if (_animator != null)
        {
            _animator.SetBool("walking", isWalking);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case NpcState.Patrol:
                PatrolLogic();
                break;
            case NpcState.Talk:
                TalkLogic();
                break;
            case NpcState.Follow:
                MoveToPlayerLogic();
                break;
        }
    }

    void MoveToPlayerLogic()
    {
        if (playerTransform == null) return;
        agent.updateRotation = true; 
        agent.SetDestination(playerTransform.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartConversation();
        }
    }

    void StartConversation()
    {
        ChangeState("Talk");
        if (_talkingVoice != null) _talkingVoice.Interact();
    }

    public void EndConversation()
    {
        ChangeState("Idle");
    }

    void PatrolLogic()
    {
        agent.updateRotation = true; 
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    IEnumerator WaitAndMove()
    {
        NpcState originalState = currentState;
        currentState = NpcState.Idle;
        
        UpdateAnimation(false); // Si ferma durante l'attesa al waypoint
        
        yield return new WaitForSeconds(waitTimeAtPoint);
        
        if (currentState == NpcState.Idle)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            currentState = originalState;
            
            UpdateAnimation(true); // Riprende a camminare
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0 || agent == null) return;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        agent.isStopped = false;
    }

    void TalkLogic()
    {
        agent.isStopped = true;
        agent.updateRotation = false;
        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * lookSpeed);
            }
        }
    }

    public void Interact() 
    {
        if (currentState != NpcState.Talk) StartConversation();
        else EndConversation();
    }

    public string GetDescription() => $"{actionVerb} {npcName}";
    public void OnFocus() { if (_outline != null) _outline.enabled = true; }
    public void OnLostFocus() { if (_outline != null) _outline.enabled = false; }
    public void ReceiveItem(GrippableItem item)
    {
        //Debug.Log("l'oggetto viene distrutto!");
        ItemReceiver itemReceiver = GetComponent<ItemReceiver>();
        if(itemReceiver != null && itemReceiver.enabled) itemReceiver.ReceiveItem(item);
    }
}