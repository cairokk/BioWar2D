using System.Collections.Generic;
using UnityEngine;

public class EfeitosDatabase : MonoBehaviour
{
    public static EfeitosDatabase Instance;

    public List<CartaEfeito> todosOsEfeitos; // Todas os efeitos do jogo

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public CartaEfeito GetCartaById(int id)
    {
        return todosOsEfeitos.Find(e => e.id == id);
    }
}
