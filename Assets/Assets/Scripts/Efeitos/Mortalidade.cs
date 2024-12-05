
using UnityEngine;


[CreateAssetMenu(fileName = "Mortalidade", menuName = "Efeitos Carta/Alterad Mortalidade")]
public class Mortalidade : CartaEfeito
{
    public int qtd;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        int novaTaxaDeMortalidade = gameController.atributosVirus.taxaDeMortalidade + qtd;
        Mathf.Clamp(novaTaxaDeMortalidade, 0, 10);
        gameController.atributosVirus.taxaDeMortalidade = Mathf.Clamp(novaTaxaDeMortalidade, 0, 10);
        gameController.OnAtributosVirusChanged(gameController.atributosVirus);
    }
}