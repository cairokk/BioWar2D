using System.Collections.Generic;
using UnityEngine;

public static class EfeitoRegistry
{
    private static Dictionary<int, CartaEfeito> efeitosRegistrados = new Dictionary<int, CartaEfeito>();

    // Método para registrar um efeito
    public static void RegistrarEfeito(CartaEfeito efeito)
    {
        if (efeito == null)
        {
            Debug.LogError("Tentativa de registrar um efeito nulo.");
            return;
        }

        if (!efeitosRegistrados.ContainsKey(efeito.id))
        {
            efeitosRegistrados.Add(efeito.id, efeito);
            Debug.Log($"Efeito registrado: {efeito.name} (ID: {efeito.id})");
        }
        else
        {
            Debug.LogWarning($"O efeito {efeito.name} já está registrado.");
        }
    }

    // Método para obter um efeito pelo ID
    public static CartaEfeito ObterEfeito(int id)
    {
        if (efeitosRegistrados.TryGetValue(id, out var efeito))
        {
            return efeito;
        }

        Debug.LogWarning($"Efeito não encontrado para o ID: {id}");
        return null;
    }

    // Método para registrar todos os efeitos de forma automática
    public static void RegistrarTodosEfeitos()
    {
        CartaEfeito[] todosEfeitos = Resources.LoadAll<CartaEfeito>("Efeitos");
        foreach (var efeito in todosEfeitos)
        {
            RegistrarEfeito(efeito);
        }
    }
}
