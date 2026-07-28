using UnityEngine;

public class PanelReTruco : MonoBehaviour
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
