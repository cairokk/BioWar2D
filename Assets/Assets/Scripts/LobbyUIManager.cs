using Edgegap;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : NetworkBehaviour
{
    public Button switchTeamButton;
    public Button inciarJogo;
    private PlayerController playerController;
    public GameObject vacinaRegion;
    public GameObject virusRegion;

    [Scene]
    [Tooltip("Nome da cena para a qual todos os jogadores serão transportados")]
    public string targetScene;  // Nome da cena para a qual queremos transportar os jogadores

    // Esse método será chamado pelo servidor quando o botão for clicado

    public void OnClickRequestSceneTransition()
    {
        if (playerController.isLocalPlayer) // Garante que apenas o cliente local envia o comando
        {
            CmdRequestSceneTransition();
        }
    }

    // O servidor recebe o comando e executa a transição de cena para todos os clientes
    [Command]
    void CmdRequestSceneTransition()
    {
        TransportPlayersToScene();
    }

    [Server]
    public void TransportPlayersToScene()
    {
        // Envia a mensagem de carregamento de cena para todos os jogadores conectados
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (conn != null)
            {
                SceneMessage message = new SceneMessage { sceneName = targetScene, sceneOperation = SceneOperation.LoadAdditive };
                conn.Send(message);
            }
        }
    }

    void Start()
    {
        switchTeamButton.onClick.AddListener(OnSwitchTeamClicked);
        inciarJogo.onClick.AddListener(OnClickRequestSceneTransition);
    }

    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;
        UpdateTeamHighlight();
    }


    void OnSwitchTeamClicked()
    {

        if (playerController != null)
        {
            playerController.CmdSwitchTeamsForAll();
        }
        else
        {
            Debug.LogWarning("PlayerController não encontrado!");
        }
    }

    public void UpdateTeamHighlight()
    {
        if (playerController == null) return;

        if (playerController.playerTeam == "vacina")
        {
            HighlightRegion(vacinaRegion, true);
            HighlightRegion(virusRegion, false);
        }
        else
        {
            HighlightRegion(vacinaRegion, false);
            HighlightRegion(virusRegion, true);
        }
    }

    private void HighlightRegion(GameObject region, bool highlight)
    {
        Image regionImage = region.GetComponent<Image>();
        if (regionImage != null)
        {
            // Defina uma cor para destacar e outra para quando estiver sem destaque
            regionImage.color = highlight ? Color.green : Color.white; // Altere as cores conforme necessário
        }
    }

    void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Jogo");
    }


}
