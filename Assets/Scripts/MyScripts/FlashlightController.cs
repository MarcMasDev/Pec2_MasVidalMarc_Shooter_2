using System;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    private float battery = 100f;
    private bool inUse = false;
    [SerializeField] private float maxBattery = 100f;
    public float consumptionRate = 0f;
    public float recoverRate = 0f;
    private bool isOn = false;

    public GameObject[] flashlights;
    public GameObject volume;
    public CharacterBlackboard m_characterBlackboard;
    public static Action<bool> OnFlashlight;
    private void Start()
    {
        battery = maxBattery;
    }
    private void OnDisable()
    {
        if (isOn) SetFlashlight(false);
    }

    private void Update()
    {
        bool input = m_characterBlackboard.GetPlayerInput().IsFlashlight;

        if (!inUse && input && battery >= maxBattery)
        {
            inUse = true;
            SetFlashlight(true);
        }
        else if (inUse && !input)
        {
            inUse = false;
            SetFlashlight(false);
        }


        if (inUse)
        {
            if (battery > 0)
            {
                battery -= consumptionRate * Time.deltaTime;
            }
            else SetFlashlight(false);
        }
        else if (battery < maxBattery)
        {
            battery += recoverRate * Time.deltaTime;
        }

    }

    private void SetFlashlight(bool on)
    {
        for (int i = 0; i < flashlights.Length; i++)
        {
            flashlights[i].SetActive(on);
        }
        volume.SetActive(on);
        isOn = on;
        OnFlashlight?.Invoke(on);
    }
    public float GetCharge()
    {
        return battery/maxBattery;
    }
}
