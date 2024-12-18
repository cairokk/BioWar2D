using System.Collections;
using Edgegap;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    

    public void OnButtonClicked()
    {

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null && isServer)
        {
            StartCoroutine(SendPlayerToNewScene(player));
        }
        else
        {
            Debug.LogWarning("Player object is not assigned or not on server.");
        }
    }

    void Start()
    {
        switchTeamButton.onClick.AddListener(OnSwitchTeamClicked);
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

    [ServerCallback]
    IEnumerator SendPlayerToNewScene(GameObject player)
    {
        if (player.TryGetComponent<NetworkIdentity>(out NetworkIdentity identity))
        {
            NetworkConnectionToClient conn = identity.connectionToClient;
            if (conn == null) yield break;

            conn.Send(new SceneMessage { sceneName = this.gameObject.scene.path, sceneOperation = SceneOperation.UnloadAdditive, customHandling = true });

            NetworkServer.RemovePlayerForConnection(conn, RemovePlayerOptions.Destroy);

            SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByPath(targetScene));
            conn.Send(new SceneMessage{sceneName = targetScene, sceneOperation = SceneOperation.LoadAdditive, customHandling = true});

            NetworkServer.AddPlayerForConnection(conn,player);
            
        }



    }


}
