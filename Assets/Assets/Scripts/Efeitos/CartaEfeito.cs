using UnityEngine;

public abstract class CartaEfeito : ScriptableObject
{
    public abstract void ApplyEffect(PlayerController player, GameController gameController);
}