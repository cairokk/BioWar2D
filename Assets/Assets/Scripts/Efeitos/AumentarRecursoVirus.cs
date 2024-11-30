
using UnityEngine;


[CreateAssetMenu(fileName = "AumentarRecursoVirus", menuName = "Efeitos Carta/Aumetar Recurso Virus")]
public class AumentarRecursoVirus : CartaEfeito
{
    public int qtdRecurso;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        gameController.atributosVirus.recurso += qtdRecurso;
        Debug.Log($"{player.name} aumentou {qtdRecurso} de recurso do virus.");
    }
}