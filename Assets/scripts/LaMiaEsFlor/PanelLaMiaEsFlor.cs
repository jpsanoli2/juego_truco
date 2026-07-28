using UnityEngine;

public class PanelLaMiaEsFlor : MonoBehaviour
{
    [SerializeField] private GameObject panelFlor;
    public void MostrarPanel()
    {
        panelFlor.SetActive(true);
    }
    public void OcultarPanel()
    {
        panelFlor.SetActive(false);
    }
}
