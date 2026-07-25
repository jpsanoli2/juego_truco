using UnityEngine;
using UnityEngine.UI;

public class NoQuieroReEnvido : MonoBehaviour
{
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlNoQuerer);
    }
    private void AlNoQuerer()
    {
        GameManager.Instance.ProcesarReEnvidoRpc(EnumReEnvido.noQuiero);
    }
}
