using UnityEngine;
using UnityEngine.UI;

public class ContraFlorNoQuiero : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(ContraFlorNoQuieroR);
    }
    private void ContraFlorNoQuieroR()
    {
        GameManager.Instance.ProcesarRespuestaContraFlorRpc(EnumContraFlor.noQuiero);
    }
}
