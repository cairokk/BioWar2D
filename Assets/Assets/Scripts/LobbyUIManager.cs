using System.Collections;
using Edgegap;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening; // Certifique-se de importar o namespace DoTween
public class LobbyUIManager : NetworkBehaviour
{
    public AudioSource audioSource; // Referência ao AudioSource
    public AudioClip switchSound;   // Som do botão
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
    if (audioSource != null && switchSound != null)
    {
        audioSource.PlayOneShot(switchSound); // Toca o som ao pressionar o botão
    }

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
    RectTransform rectTransform = region.GetComponent<RectTransform>();
    Image regionImage = region.GetComponent<Image>();

    if (rectTransform != null)
    {
        Vector3 targetScale = highlight ? new Vector3(1.05f, 1.05f, 1f) : Vector3.one;
        float duration = 0.3f;

        rectTransform.DOScale(targetScale, duration).SetEase(Ease.OutBack); // Animação suave de escala
    }

    if (regionImage != null)
    {
        float targetAlpha = highlight ? 1f : 0.6f; // 1 para visível, 0.6 para semidirecionado
        float duration = 0.3f;

        // Interpolação suave da opacidade usando DOFade
        regionImage.DOFade(targetAlpha, duration);
    }
}



}
