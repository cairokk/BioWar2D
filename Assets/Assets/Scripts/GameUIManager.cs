using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI curaText;
    [SerializeField] private TextMeshProUGUI virusText;
    [SerializeField] private TextMeshProUGUI playerDeck;
    [SerializeField] private TextMeshProUGUI playerDiscarte;
    [SerializeField] private TextMeshProUGUI enemyDeck;
    [SerializeField] private TextMeshProUGUI enemyDiscarte;

    [SerializeField] private TextMeshProUGUI curaRecursos;
    [SerializeField] private TextMeshProUGUI virusRecursos;


    public GameObject historyPanel; 
    public GameObject OpenButton; 
    public GameObject CloseButton; 

    public GameObject DeckBuildPanel; 
    public GameObject OpenButtonDeckBuild; 
    public GameObject CloseButtonDeckBuild; 
    [SerializeField] private RectTransform historyPanelRect;
    [SerializeField] private RectTransform toggleButtonRect; 

    private GameController gameController;
    private string lastCuraText = "";
    private string lastCuraRecursoText = "";
    private string lastVirusText = "";
    private string lastVirusRecursoText = "";
    private Vector2 panelOffScreenPosition  = new Vector2(-164, -129); // Ajuste conforme necessário.
    private Vector2 panelOnScreenPosition  = new Vector2(-58, -129);    
    private Vector2 buttonOffScreenPosition = new Vector2(-102, -134); // Ajuste conforme necessário
    private Vector2 buttonOnScreenPosition = new Vector2(4, -134); // Posição ao lado do painel
    private bool isPanelOpen = false; // Controle do estado do painel
    private void Start()
    {
        // Procura pelo GameController na cena
        gameController = FindObjectOfType<GameController>();
        historyPanelRect.anchoredPosition = panelOffScreenPosition;
        toggleButtonRect.anchoredPosition = buttonOffScreenPosition;

        if (gameController == null)
        {
            Debug.LogError("GameController não encontrado!");
        }
    }

    private void Update()
    {
        // Atualiza o painel de atributos da cura
        if (gameController != null && gameController.atributosCura != null)
        {
            string newCuraText = $"Taxa de Cura: {gameController.atributosCura.taxaDacura}\n" +
                                 $"Taxa de Pesquisa: {gameController.atributosCura.taxaDePesquisa}\n" +
                                 $"Fator de Urgência: {gameController.atributosCura.fatorDeUrgencia}\n" +
                                 $"Avanço da Cura: {gameController.atributosCura.avancoDaCura}\n";


            string newCuraRecursoText = $"{gameController.atributosCura.recurso}";

            if (newCuraText != lastCuraText || newCuraRecursoText != lastCuraRecursoText)
            {
                curaText.text = newCuraText;
                lastCuraText = newCuraText;
                curaRecursos.text = newCuraRecursoText;
                lastCuraRecursoText = newCuraRecursoText;
            }
        }

        // Atualiza o painel de atributos do vírus
        if (gameController != null && gameController.atributosVirus != null)
        {
            string newVirusText = $"Taxa de Mortalidade: {gameController.atributosVirus.taxaDeMortalidade}\n" +
                                  $"Taxa de Infecção: {gameController.atributosVirus.taxaDeInfeccao}\n";
                                 

            string newVirusRecursoText = $"{gameController.atributosVirus.recurso}";
            if (newVirusText != lastVirusText)
            {
                virusText.text = newVirusText;
                lastVirusText = newVirusText;
                virusRecursos.text = newVirusRecursoText;
                
            }
        }
        
    }

    public void UpdateUI(int playerDeck, int playerDiscarte, int enemyDeck, int enemyDiscarte)
    {
        Debug.Log("Atualizei a interface dos baralhos");

        this.playerDeck.text = playerDeck.ToString();
        this.playerDiscarte.text = playerDiscarte.ToString();
        this.enemyDeck.text = enemyDeck.ToString();
        this.enemyDiscarte.text = enemyDiscarte.ToString();
    }
    public void ToggleHistoryPanel()
        {
            if (isPanelOpen)
            {
                // Fecha o painel
                historyPanelRect.DOAnchorPos(panelOffScreenPosition, 0.5f);
                toggleButtonRect.DOAnchorPos(buttonOffScreenPosition, 0.5f);
            }
            else
            {
                // Abre o painel
                historyPanelRect.DOAnchorPos(panelOnScreenPosition, 0.5f);
                toggleButtonRect.DOAnchorPos(buttonOnScreenPosition, 0.5f);
            }

            // Alterna o estado
            isPanelOpen = !isPanelOpen;
        }
        public void OpenDeckBuild()
    {
        if (DeckBuildPanel != null)
        {
            DeckBuildPanel.SetActive(true); 
            DeckBuildPanel.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack); // Animação de escala de 0 para 1
            OpenButtonDeckBuild.SetActive(true); 
            CloseButtonDeckBuild.SetActive(true); 
        }
        else
        {
            Debug.LogWarning("DeckBuildPanel é nulo!");
        }
    }
    public void CloseDeckbuild()
{
    if (DeckBuildPanel != null)
    {
        DeckBuildPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => 
        {
            DeckBuildPanel.SetActive(false); 
        });
        OpenButtonDeckBuild.SetActive(true); 
        CloseButtonDeckBuild.SetActive(false); 
    }
    else
    {
        Debug.LogWarning("DeckBuildPanel é nulo!");
    }
}
}
