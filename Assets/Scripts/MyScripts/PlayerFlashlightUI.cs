using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFlashlightUI : MonoBehaviour
{
    [SerializeField] private Slider flashlightSlider;
    [SerializeField] private FlashlightController flashlights;
    [SerializeField] private Image fill;
    [SerializeField] private Color fullyChargedColor;
    private Color initcolor;

    private void Awake()
    {
        initcolor = fill.color;
    }

    private void Update()
    {
        if (flashlights == null && flashlightSlider == null) return;

        flashlightSlider.value = flashlights.GetCharge();


        if (flashlightSlider.value <= flashlightSlider.maxValue) fill.color = fullyChargedColor;
        else fill.color = initcolor;
    }
}
