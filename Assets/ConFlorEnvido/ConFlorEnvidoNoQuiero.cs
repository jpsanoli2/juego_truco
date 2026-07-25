using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConFlorEnvidoNoQuiero : MonoBehaviour
{
    private Button boton;
    [SerializeField] private TextMeshProUGUI textoEnvido;


    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlNoQuerer);
    }
    private void AlNoQuerer()
    {
        GameManager.Instance.ProcesarRespuesConFlorEnvidoRpc(EnumConFlorEnvido.noQuiero);
    }
}
