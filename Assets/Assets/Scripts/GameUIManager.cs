using TMPro;
using UnityEngine;

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

    private GameController gameController;
    private string lastCuraText = "";
    private string lastVirusText = "";

    private void Start()
    {
        // Procura pelo GameController na cena
        gameController = FindObjectOfType<GameController>();

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

            if (newCuraText != lastCuraText)
            {
                curaText.text = newCuraText;
                lastCuraText = newCuraText;
                curaRecursos.text = newCuraRecursoText;
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
    public void OpenHistory()
    {
        if (historyPanel != null)
        {
            historyPanel.SetActive(true);
            OpenButton.SetActive(false);
            CloseButton.SetActive(true);


        }
    }

   
     public void CloseHistory()
    {
        if (historyPanel != null)
        {
            historyPanel.SetActive(false);
            CloseButton.SetActive(false);
            OpenButton.SetActive(true);


        }
    }

     public void OpenDeckBuild()
    {
        if (DeckBuildPanel != null)
        {
            DeckBuildPanel.SetActive(true);
            OpenButtonDeckBuild.SetActive(true);
            CloseButtonDeckBuild.SetActive(true);


        }
    }
    public void CloseDeckbuild()
{
    if (DeckBuildPanel != null)
    {
        DeckBuildPanel.SetActive(false); // Fecha o painel
        OpenButtonDeckBuild.SetActive(true); // Mostra o botão de abrir
        CloseButtonDeckBuild.SetActive(false); // Esconde o botão de fechar
    }
    else
    {
        Debug.LogWarning("DeckBuildPanel é nulo!");
    }
}
}
