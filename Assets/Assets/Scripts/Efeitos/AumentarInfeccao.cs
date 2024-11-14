
using UnityEngine;


[CreateAssetMenu(fileName = "AumentarInfeccao", menuName = "Efeitos Carta/Aumetar Infeccao")]
public class AumentarInfeccao : CartaEfeito
{
    public int qtdInfeccao;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        //player.Heal(qtdInfeccao);
        Debug.Log($"{player.name} aumentou {qtdInfeccao} de infecção.");
    }
}