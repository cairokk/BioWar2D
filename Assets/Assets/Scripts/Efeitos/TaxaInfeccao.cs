
using UnityEngine;


[CreateAssetMenu(fileName = "TaxaInfeccao", menuName = "Efeitos Carta/Alterar Taxa Infeccao")]
public class TaxaInfeccao : CartaEfeito
{
    public int qtdInfeccao;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        int taxaDeInfeccao = gameController.atributosVirus.taxaDeInfeccao;
        gameController.atributosVirus.taxaDeInfeccao = taxaDeInfeccao > 10 ? taxaDeInfeccao : taxaDeInfeccao + qtdInfeccao;
        gameController.OnAtributosVirusChanged(gameController.atributosVirus);
        Debug.Log($"{player.name} aumentou {qtdInfeccao} de infecção.");
    }
}