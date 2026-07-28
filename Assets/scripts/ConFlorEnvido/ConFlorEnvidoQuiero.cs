using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConFlorEnvidoQuiero : MonoBehaviour
{
    private Button boton;
    [SerializeField] private TextMeshProUGUI textoEnvido;


    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlQuerer);
    }
    private void AlQuerer()
    {
        GameManager.Instance.ProcesarRespuesConFlorEnvidoRpc(EnumConFlorEnvido.quiero);
    }
}
