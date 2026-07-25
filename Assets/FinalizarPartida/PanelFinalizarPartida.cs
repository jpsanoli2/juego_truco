using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PanelFinalizarPartida : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI texto;

    public void MostrarPanel(string txt)
    {
        MostrarTexto(txt);
        panel.SetActive(true);
    }
    public void OcultarPanel()
    {
        panel.SetActive(false);
    }
    private void MostrarTexto(string txt)
    {
        texto.text = txt;
    }
     
}
