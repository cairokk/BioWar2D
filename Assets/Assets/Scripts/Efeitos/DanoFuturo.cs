using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "DanoFuturo", menuName = "Efeitos Carta/alterar Dano Futuro")]
public class DanoFuturo : CartaEfeito
{
    public int qtd;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        BaseController regiaoSelecionada = gameController.bases.Find(b => b.regiao.nomeBase == player.baseSelecionada);
        int novoDano = regiaoSelecionada.regiao.DanoFuturo + qtd;
        regiaoSelecionada.regiao.nivelInfecao = Mathf.Clamp(novoDano, 0, 10);
        gameController.OnAtributosVirusChanged(gameController.atributosVirus);
        
    }

}