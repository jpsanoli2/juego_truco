using UnityEngine;
using UnityEngine.UI;

public class ContraFlor : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(ContraFlorAlResto);
    }
    private void ContraFlorAlResto()
    {
        GameManager.Instance.ProcesarRespuestaFlorRpc("contraFlor");
    }
}
