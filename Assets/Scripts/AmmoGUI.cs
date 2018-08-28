using UnityEngine;
using UnityEngine.UI;

// Zeigt Anzahl der Raketen an

public class AmmoGUI : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }


    void Update()
    {

            text.text = string.Format("LRA:\nMSA:");

    }
}
