using UnityEngine;
using UnityEngine.UI;

public class LaMiaEsFlorContraFlor : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(ContraFlorAlResto);
    }
    private void ContraFlorAlResto()
    {
        GameManager.Instance.LaMiaEsFlorRpc(EnumLaMiaEsFlor.resto);
    }
}
