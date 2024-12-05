using UnityEngine;


[CreateAssetMenu(fileName = "TaxaPesquisa", menuName = "Efeitos Carta/Alterar Taxa Pesquisa")]
public class TaxaPesquisa : CartaEfeito
{
    public int qtd;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        gameController.atributosCura.taxaDePesquisa += qtd;
        Debug.Log($"{player.name} aumentou {qtd} de recurso da Cura.");
    }
}