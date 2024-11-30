
using UnityEngine;


[CreateAssetMenu(fileName = "AumentarRecursoCura", menuName = "Efeitos Carta/Aumetar Recurso Cura")]
public class AumentarRecursoCura : CartaEfeito
{
    public int qtdRecurso;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        gameController.atributosCura.recurso += qtdRecurso;
        Debug.Log($"{player.name} aumentou {qtdRecurso} de recurso da Cura.");
    }
}