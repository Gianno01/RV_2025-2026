using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private string animationTrigger = "Interacted"; // Il nome del trigger nell'Animator

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Questa funzione verrà chiamata dal Player
    public void Interact()
    {
        if (_animator != null)
        {
            // Cambia l'animazione (assicurati di avere un Trigger chiamato così nell'Animator)
            _animator.SetTrigger(animationTrigger);
            Debug.Log("Interazione avvenuta con: " + gameObject.name);
        }
    }
}