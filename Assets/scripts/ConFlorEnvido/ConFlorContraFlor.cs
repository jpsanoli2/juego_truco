using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConFlorContraFlor : MonoBehaviour
{
    private Button boton;
    [SerializeField] private TextMeshProUGUI textoEnvido;


    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlContraFlor);
    }
    private void AlContraFlor()
    {
        GameManager.Instance.ProcesarRespuesConFlorEnvidoRpc(EnumConFlorEnvido.contraFlor);
    }
}
