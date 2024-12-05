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

    public void StartTurn(TurnState turn)
    {
        {

            currentTurn = turn;
            switch (currentTurn)
            {
                case TurnState.TurnoVirus:
                    StartVirusTurn();
                    break;
                case TurnState.TurnoCura:
                    StartCuraTurn();
                    break;
                case TurnState.EventosFinaisDeTurno:
                    ExecuteEndOfTurnEvents();
                    break;
            }
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

   
    private void StartVirusTurn()
    {
        currentTurn = TurnState.TurnoVirus;
        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            player.StartTurnCheckDeckBuild();
        }
    }
 
    private void StartCuraTurn()
    {

        currentTurn = TurnState.TurnoCura;
        Debug.Log("Turn Cura");
        Debug.Log("Turn Cura");
        Debug.Log("Turn Cura");
        Debug.Log("Turn Cura");
        Debug.Log("Turn Cura");
        Debug.Log("Turn Cura");
        Debug.Log(players);
        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            
            player.StartTurnCheckDeckBuild();
            
        }
        //StartTurnCheckDeckBuild
    }


    private void ExecuteEndOfTurnEvents()
    {

        AplicarDanoAsRegioes(gameController.bases);
        AplicarAumentoDeInfeccao(gameController.bases);
        AplicarAvancoDaCura();
        AtualizarRegioesUI(gameController.bases);
        CheckVictoryCondition(gameController.bases);
        StartTurn(TurnState.TurnoVirus); // Começa uma nova rodada

    }

    private void AplicarDanoAsRegioes(List<BaseController> regioes)
    {
        foreach (var componente in regioes)
        {
            componente.regiao.CalcularDanoDaRodada();
        }
    }

    private void AtualizarRegioesUI(List<BaseController> regioes)
    {
        foreach (var componente in regioes)
        {
            componente.UpdateUI();
        }
    }

    private void AplicarAumentoDeInfeccao(List<BaseController> regioes)
    {
        foreach (var componente in regioes)
        {
            componente.regiao.CalcularNivelInfeccao(gameController.atributosVirus);
        }

    }
    private void AplicarAvancoDaCura()
    {
        gameController.atributosCura.CalcularAvancoDaCura();
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
