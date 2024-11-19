
using UnityEngine;


[CreateAssetMenu(fileName = "AumentarRecurso", menuName = "Efeitos Carta/Gerar Recurso")]
public class GerarRecursoCientifico : CartaEfeito
{
    public int qtdRecurso;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        //player.Heal(qtdInfeccao);
        Debug.Log($"{player.name} aumentou {qtdRecurso} de recuros.");
    }
}