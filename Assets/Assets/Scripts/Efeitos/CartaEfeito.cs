using UnityEngine;

public abstract class CartaEfeito : ScriptableObject
{
    public int id;
    public abstract void ApplyEffect(PlayerController player, GameController gameController);
}