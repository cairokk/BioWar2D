using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MenuInicialManeger : MonoBehaviour
{
    private NetworkManager networkManager;

    void Start()
    {
        // Tenta obter o NetworkManager como singleton
        networkManager = NetworkManager.singleton;

        if (networkManager == null)
        {
            Debug.LogError("NetworkManager não encontrado na cena.");
            return;
        }
    }
    public void CriarPartida()
    {
        Debug.Log("Criando Partida como Host...");
        NetworkManager.singleton.StartHost();
    }

    public void EntrarNaPartida()
    {

        Debug.Log("Conectando como Cliente...");
        NetworkManager.singleton.StartClient();

    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do Jogo...");
        Application.Quit();
    }
}
