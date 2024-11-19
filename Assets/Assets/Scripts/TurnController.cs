using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TurnController : NetworkBehaviour
{
    public enum TurnState
    {
        TurnoVirus,
        TurnoCura,
        EventosFinaisDeTurno
    }

    private List<PlayerController> players;

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

        // Verifica condições de vitória e executa outros eventos finais
        CheckVictoryCondition();
        StartTurn(TurnState.TurnoVirus); // Começa um novo turno

    }

    [Server]
    private void CheckVictoryCondition()
    {
        bool virusWins = false;
        bool curaWins = false;

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
