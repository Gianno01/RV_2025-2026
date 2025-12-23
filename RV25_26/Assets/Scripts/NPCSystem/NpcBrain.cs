using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum NpcState { Idle, Patrol, Talk }

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))] 
public class NpcBrain : MonoBehaviour, IsInteractable
{
    [Header("Stato")]
    public NpcState currentState = NpcState.Idle;
    private TalkingCharacter _talkingVoice; // Riferimento al componente audio
    [Header("Pattugliamento")]
    public Transform[] waypoints;
    public float waitTimeAtPoint = 2f;
    private int currentWaypointIndex = 0;

    [Header("Configurazione")]
    // Velocità di rotazione quando guarda il player
    public float lookSpeed = 5f; 
    [Header("Info Ui")]
    public string npcName = "Nome NPC";
    public string actionVerb = "Parla con";

    private NavMeshAgent agent;
    private Transform playerTransform;
    private NpcState previousState; 

  void Start()
{
    // 1. Recupera i componenti necessari
    agent = GetComponent<NavMeshAgent>();
    _talkingVoice = GetComponent<TalkingCharacter>(); // Assicurati di aver dichiarato questa variabile in alto nella classe

    // 2. Trova il Player (DICHIARAZIONE UNICA)
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    
    if (player != null) 
    {
        playerTransform = player.transform;
    }

    // 3. Configurazione NavMesh
    agent.updateRotation = true;

    if (waypoints.Length > 0)
    {
        currentState = NpcState.Patrol;
        MoveToNextWaypoint();
    }
}

    
    public string GetDescription()
    {
        return $"{actionVerb} {npcName}";
    }

    void Update()
    {
        switch (currentState)
        {
            case NpcState.Idle:
                break;

            case NpcState.Patrol:
                PatrolLogic();
                break;

            case NpcState.Talk:
                TalkLogic();
                break;
        }
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
        currentState = NpcState.Idle;
        yield return new WaitForSeconds(waitTimeAtPoint);
        
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        MoveToNextWaypoint();
        
        currentState = NpcState.Patrol;
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        agent.isStopped = false;
    }

    void TalkLogic()
    {
        agent.isStopped = true;
        agent.updateRotation = false; // Disabilitiamo la rotazione automatica del pathfinding

        // LOGICA LOOK-AT (Ruota verso il player)
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0; // Mantiene il cilindro dritto, ruota solo su Y
            
            if (direction != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * lookSpeed);
            }
        }
    }

    // --- INTERAZIONE ---

    public void Interact()
    {
        if (currentState != NpcState.Talk)
            StartConversation();
        else
            EndConversation();
    }

    public void StartConversation()
    {
        previousState = currentState;
        currentState = NpcState.Talk;

        // COMANDO ALLA VOCE: Se esiste il componente audio, fallo suonare
        if (_talkingVoice != null)
        {
            _talkingVoice.Interact();
        }

        Debug.Log("NPC: Inizio conversazione.");
    }

    public void EndConversation()
    {
        Debug.Log("NPC: Arrivederci.");
        currentState = previousState;
        if (currentState == NpcState.Patrol) agent.isStopped = false;
    }

    // Implementazione vuota richiesta dall'interfaccia
    public void OnFocus() {}
    public void OnLostFocus() {}

    // --- DEBUG VISIVO ---
    // Disegna una riga rossa nella Scene View per farti capire dov'è il "davanti" del cilindro
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 2;
        Gizmos.DrawRay(transform.position + Vector3.up, direction);
        
        // Disegna una sfera sopra la testa per trovarlo facilmente
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 2, 0.2f);
    }
}
