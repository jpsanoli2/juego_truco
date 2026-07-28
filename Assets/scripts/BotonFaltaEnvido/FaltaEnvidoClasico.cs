using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FaltaEnvidoClasico : MonoBehaviour
{
    private Button boton;
   


    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlClickear);
    }

    private void AlClickear()
    {
        GameManager.Instance.ProcesarFaltaEnvidoRpc();
    }
}
