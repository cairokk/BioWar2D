
using UnityEngine;


[CreateAssetMenu(fileName = "AumentarMortalidade", menuName = "Efeitos Carta/Aumetar Mortalidade")]
public class AumentarMortalidade : CartaEfeito
{
    public int qtdMortalidade;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        int taxaDeMortalidade = gameController.atributosVirus.taxaDeMortalidade;
        gameController.atributosVirus.taxaDeMortalidade = taxaDeMortalidade > 10 ? taxaDeMortalidade : taxaDeMortalidade + qtdMortalidade;
        gameController.OnAtributosVirusChanged(gameController.atributosVirus);
        Debug.Log($"{player.name} aumentou {qtdMortalidade} de infecção.");
    }
}