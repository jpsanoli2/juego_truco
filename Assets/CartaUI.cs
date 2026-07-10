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
        // Le decimos al botón que cuando lo clickeen ejecute nuestra función
        boton.onClick.AddListener(AlClickearCarta);
    }

    private void AlClickearCarta()
    {
        if (string.IsNullOrEmpty(nombreCartaAsignada)) return;

        Debug.Log($"Clické la carta: {nombreCartaAsignada}. Avisando al GameManager...");

        // Buscamos el GameManager y le pasamos el nombre de la carta que queremos jugar
        //GameManager.Instance.IntentarJugarCartaServerRpc(nombreCartaAsignada);
    }
}