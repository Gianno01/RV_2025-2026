using UnityEngine;

// Questa interfaccia è il contratto comune per tutto ciò che è interattivo
public interface IsSelectable
{
    void select();
    string GetDescription(); // Utile per mostrare "Premi E per aprire"
}