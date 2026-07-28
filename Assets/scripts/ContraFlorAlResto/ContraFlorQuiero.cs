using UnityEngine;
using UnityEngine.UI;

public class ContraFlorQuiero : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(ContraFlorQuieroR);
    }
    private void ContraFlorQuieroR()
    {
        GameManager.Instance.ProcesarRespuestaContraFlorRpc(EnumContraFlor.quiero);
    }
}
