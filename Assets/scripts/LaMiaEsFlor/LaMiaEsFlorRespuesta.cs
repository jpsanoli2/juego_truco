using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaMiaEsFlorRespuesta : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(LaMiaEsFlor);
    }
    private void LaMiaEsFlor()
    {
        GameManager.Instance.LaMiaEsFlorRpc(EnumLaMiaEsFlor.flor);
    }
}
