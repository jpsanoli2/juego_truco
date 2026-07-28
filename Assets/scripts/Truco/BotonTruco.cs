using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotonTruco : MonoBehaviour
{
    private Button boton;
    [SerializeField] private TextMeshProUGUI texto;


    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlClickear);
    }

    private void AlClickear()
    {
        GameManager.Instance.ProcesarTrucoRpc();
    }
    public void TextoTruco()
    {
        texto.text = "Truco";
    }
    public void TextoReTruco()
    {
        texto.text = "Re truco";
    }
    public void TextoValeCuatro()
    {
        texto.text = "Vale cuatro";
    }
}
