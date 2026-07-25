using UnityEngine;
using UnityEngine.UI;

public class ConFlorEnvido : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(FlorEnvido);
    }
    private void FlorEnvido()
    {
        GameManager.Instance.ProcesarRespuestaFlorRpc("envido");
    }
}
