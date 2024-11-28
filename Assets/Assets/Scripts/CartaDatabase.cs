using System.Collections.Generic;
using UnityEngine;

public class CartaDatabase : MonoBehaviour
{
    public static CartaDatabase Instance;

    public List<Carta> todasAsCartas; // Todas as cartas do jogo

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Carta GetCartaById(int id)
    {
        return todasAsCartas.Find(c => c.id == id);
    }
}
