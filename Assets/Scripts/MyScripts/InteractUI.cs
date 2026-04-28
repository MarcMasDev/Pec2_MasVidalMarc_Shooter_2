using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private void Awake()
    {
        Show("");
    }
    public void Show(string txt)
    {
        text.text = txt;
    }
}
