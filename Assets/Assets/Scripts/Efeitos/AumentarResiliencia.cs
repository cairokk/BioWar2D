using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "AumentarResiliencia", menuName = "Efeitos Carta/Aumetar Resiliencia")]
public class AumentarResiliencia : CartaEfeito
{
    public int qtdResiliencia;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        gameController.StartCoroutine(SelecionarBaseEAplicarEfeito(player, gameController));
    }

    private IEnumerator SelecionarBaseEAplicarEfeito(PlayerController player, GameController gameController)
    {
        gameController.selecionandoBase = true;

        while (string.IsNullOrEmpty(gameController.baseSelecionada))
        {
            yield return null; 
        }

        BaseController baseSelecionada = gameController.bases.Find(b => b.regiao.nomeBase == gameController.baseSelecionada);

        baseSelecionada.regiao.defesa += qtdResiliencia;

        gameController.baseSelecionada = "";
        gameController.selecionandoBase = false;

        Debug.Log($"{player.name} aumentou {qtdResiliencia} de infecção.");
    }
}