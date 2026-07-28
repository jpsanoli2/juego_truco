using UnityEngine;
using UnityEngine.UI;

public class CartaUI : MonoBehaviour
{
    // Guardamos el nombre o ID de la carta que tiene asignada actualmente
    public string nombreCartaAsignada;
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlClickearCarta);
    }

    private void AlClickearCarta()
    {
        GameManager.Instance.IntentarJugarCartaRpc(nombreCartaAsignada);
    }
}