using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System; // Necessario per il parsing degli Enum

public enum NpcState { Idle, Patrol, Talk }

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))] 
public class NpcBrain : MonoBehaviour, IsInteractable, IStateChangeable
{
    [Header("Stato")]
    public NpcState currentState = NpcState.Idle;
    private TalkingCharacter _talkingVoice; 

    [Header("Pattugliamento")]
    public Transform[] waypoints;
    public float waitTimeAtPoint = 2f;
    private int currentWaypointIndex = 0;

    [Header("Configurazione")]
    public float lookSpeed = 5f; 
    [Header("Info Ui")]
    public string npcName = "Nome NPC";
    public string actionVerb = "Parla con";

    private NavMeshAgent agent;
    private Transform playerTransform;
    private NpcState previousState; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _talkingVoice = GetComponent<TalkingCharacter>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
        
        agent.updateRotation = true;

        if (waypoints.Length > 0 && currentState == NpcState.Idle)
        {
            ChangeState("Patrol");
        }
    }

    // implementazione di IStateChangeable 
    public void ChangeState(string state)
    {
        // Converte la stringa in Enum (ignora maiuscole/minuscole)
        if (Enum.TryParse(state, true, out NpcState newState))
        {
            // Interrompiamo eventuali routine di attesa in corso per evitare sovrapposizioni
            StopAllCoroutines();
            
            currentState = newState;

            // Logica di attivazione immediata basata sul nuovo stato
            switch (currentState)
            {
                case NpcState.Patrol:
                    agent.isStopped = false;
                    MoveToNextWaypoint();
                    break;
                case NpcState.Idle:
                case NpcState.Talk:
                    agent.isStopped = true;
                    break;
            }
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
        NpcState originalState = currentState;
        currentState = NpcState.Idle;
        yield return new WaitForSeconds(waitTimeAtPoint);
        
        // Se nel frattempo lo stato non è stato cambiato da una quest o interazione
        if (currentState == NpcState.Idle)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            currentState = originalState;
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

    public void StartConversation()
    {
        previousState = currentState;
        ChangeState("Talk");

        if (_talkingVoice != null) _talkingVoice.Interact();
    }

    public void EndConversation()
    {
        ChangeState(previousState.ToString());
    }

    public string GetDescription() => $"{actionVerb} {npcName}";
    public void OnFocus() {}
    public void OnLostFocus() {}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * 2);
    }
}
