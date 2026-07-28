using TMPro;
using UnityEngine;

public class PanelFaltaEnvido : MonoBehaviour
{ 
    [SerializeField] private GameObject panel;

    public void MostrarPanel()
    {
        panel.SetActive(true);
    }
    public void OcultarPanel()
    {
        panel.SetActive(false);
    }
}
