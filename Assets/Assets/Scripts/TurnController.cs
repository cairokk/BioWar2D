using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class TurnController : NetworkBehaviour
{
    public enum TurnState
    {
        TurnoVirus,
        TurnoCura,
        EventosFinaisDeTurno
    }

    private List<PlayerController> players;
    private GameController gameController;

    [SyncVar]
    public TurnState currentTurn;

    [Server]
    public void StartTurn(TurnState turn)
    {
        Debug.Log("entrei no StartTurn");

        currentTurn = turn;
        Debug.Log(turn);
        switch (currentTurn)
        {
            case TurnState.TurnoVirus:
                RpcStartVirusTurn();
                break;
            case TurnState.TurnoCura:
                RpcStartCuraTurn();
                break;
            case TurnState.EventosFinaisDeTurno:
                RpcExecuteEndOfTurnEvents();
                break;
        }
    }

    public void InitializePlayers(List<PlayerController> connectedPlayers)
    {
        players = connectedPlayers;
    }

    public void InitializeGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    [ClientRpc]
    private void RpcStartVirusTurn()
    {

        // Habilite o controle para o jogador do vírus
        currentTurn = TurnState.TurnoVirus;
        Debug.Log("É o turno do Vírus!");

    }

    [ClientRpc]
    private void RpcStartCuraTurn()
    {

        currentTurn = TurnState.TurnoCura;
        Debug.Log("É o turno da Cura!");

    }

    [ClientRpc]
    private void RpcExecuteEndOfTurnEvents()
    {

        AplicarDanoAsRegioes(gameController.bases);
        AplicarAumentoDeInfeccao(gameController.bases);
        AplicarAvancoDaCura();
        CheckVictoryCondition(gameController.bases);
        StartTurn(TurnState.TurnoVirus); // Começa uma nova rodada

    }

    private void AplicarDanoAsRegioes(List<BaseController> regioes){
        foreach (var componente in regioes){
            componente.regiao.CalcularDanoDaRodada();
        }
    }

    private void AplicarAumentoDeInfeccao(List<BaseController> regioes){
        foreach (var componente in regioes){
            componente.regiao.CalcularNivelInfeccao(gameController.atributosVirus);
        }
    }

    private void AplicarAvancoDaCura(){
        gameController.atributosCura.calcularAvancoDaCura();
    }

    private void CheckVictoryCondition(List<BaseController> regioes)
    {
        bool virusWins = false;
        bool curaWins = false;
        virusWins = !regioes.Any(r => r.regiao.vida > 0);
        curaWins = gameController.atributosCura.taxaDacura >= 10;


        if (virusWins)
        {
            RpcEndGame("Vírus venceu!");
        }
        else if (curaWins)
        {
            RpcEndGame("Cura venceu!");
        }
    }

    [ClientRpc]
    private void RpcEndGame(string message)
    {
        Debug.Log(message);
        // Mostre uma tela de final de jogo para os jogadores
    }

    [ClientRpc]
    public void RpcEndCurrentTurn()
    {
        Debug.Log("Entrei no Rpc Dentro do TurnController");
        if (currentTurn == TurnState.TurnoVirus)
        {
            StartTurn(TurnState.TurnoCura);
        }
        else if (currentTurn == TurnState.TurnoCura)
        {
            StartTurn(TurnState.EventosFinaisDeTurno);
        }
    }
}
