using UnityEngine;
using UnityEngine.UI;

public class TrucoNoQuiero : MonoBehaviour
{
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlClickear);
    }
    private void AlClickear()
    {
        GameManager.Instance.ResponderTrucoRpc(EnumTruco.noQuiero);
    }
}
