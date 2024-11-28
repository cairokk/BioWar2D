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
                                 $"Avanço da Cura: {gameController.atributosCura.avancoDaCura}";

            if (newCuraText != lastCuraText)
            {
                curaText.text = newCuraText;
                lastCuraText = newCuraText;
            }
        }

        // Atualiza o painel de atributos do vírus
        if (gameController != null && gameController.atributosVirus != null)
        {
            string newVirusText = $"Taxa de Mortalidade: {gameController.atributosVirus.taxaDeMortalidade}\n" +
                                  $"Taxa de Infecção: {gameController.atributosVirus.taxaDeInfeccao}\n";
            if (newVirusText != lastVirusText)
            {
                virusText.text = newVirusText;
                lastVirusText = newVirusText;
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
}
