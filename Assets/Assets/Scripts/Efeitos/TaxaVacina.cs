using UnityEngine;


[CreateAssetMenu(fileName = "TaxaVacina", menuName = "Efeitos Carta/Alterar Taxa Vacina")]
public class TaxaVacina : CartaEfeito
{
    public int qtd;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        int novaAvancoCura = gameController.atributosCura.avancoDaCura + qtd;
        gameController.atributosCura.avancoDaCura = Mathf.Clamp(novaAvancoCura, 0, 10);

    }
}