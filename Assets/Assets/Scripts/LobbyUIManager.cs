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


}
