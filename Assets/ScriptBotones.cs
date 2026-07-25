using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScriptBotones : MonoBehaviour
{
    [SerializeField] private Transform panelBotones;
    [SerializeField] private Button botonCalcularEnvido;
    [SerializeField] private Button botonEnvido;
    [SerializeField] private Button faltaEnvido;
    [SerializeField] private Button truco;

    public void SetInteraccionBotones(bool interactuable)
    {
        Button[] botones = panelBotones.GetComponentsInChildren<Button>();

        foreach (Button boton in botones)
        {
            boton.interactable = interactuable;
        }
    }
    public void SetInteraccionBotonesSinCalcularEnvido(bool interactuable)
    {
        Button[] botones = panelBotones.GetComponentsInChildren<Button>();

        foreach (Button boton in botones)
        {
            if (boton != botonCalcularEnvido)
            {
                boton.interactable = interactuable;
            }
        }
    }
    public void DeshabilitadBotonEnvido()
    {
        botonEnvido.interactable = false;
        faltaEnvido.interactable = false;
    }
    public void SetInteraccionBotonesGeneral(bool interactuable)
    {
        Button[] botones = panelBotones.GetComponentsInChildren<Button>();

        foreach (Button boton in botones)
        {
            if (boton != botonCalcularEnvido && boton != faltaEnvido && boton != botonEnvido)
                
            {
                boton.interactable = interactuable;
            }
        }
    }
}
