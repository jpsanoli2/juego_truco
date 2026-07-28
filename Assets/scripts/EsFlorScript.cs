using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EsFlorScript : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(EsFlor);
    }
    private void EsFlor()
    {
        GameManager.Instance.ProcesarRespuestaFlorRpc("flor");
    }
}

