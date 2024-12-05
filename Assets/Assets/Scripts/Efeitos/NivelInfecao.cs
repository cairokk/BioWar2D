using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "NivelInfecao", menuName = "Efeitos Carta/Alterar Nivel Infecao")]
public class NivelInfecao : CartaEfeito
{
    public int qtd;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        BaseController regiaoSelecionada = gameController.bases.Find(b => b.regiao.nomeBase == player.baseSelecionada);
        int novoNivel = regiaoSelecionada.regiao.nivelInfecao + qtd;
        regiaoSelecionada.regiao.nivelInfecao = Mathf.Clamp(novoNivel, 0, 10);
        gameController.OnAtributosVirusChanged(gameController.atributosVirus);
        
    }

}