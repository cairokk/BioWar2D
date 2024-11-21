
using UnityEngine;


[CreateAssetMenu(fileName = "AumentarInfeccao", menuName = "Efeitos Carta/Aumetar Infeccao")]
public class AumentarInfeccao : CartaEfeito
{
    public int qtdInfeccao;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        int taxaDeInfeccao = gameController.atributosVirus.taxaDeInfeccao;
        gameController.atributosVirus.taxaDeInfeccao = taxaDeInfeccao > 10 ? taxaDeInfeccao : taxaDeInfeccao + qtdInfeccao;
        gameController.atributosVirus.taxaDeMortalidade += 1;
        gameController.OnAtributosVirusChanged(gameController.atributosVirus);
        Debug.Log($"{player.name} aumentou {qtdInfeccao} de infecção.");
    }
}