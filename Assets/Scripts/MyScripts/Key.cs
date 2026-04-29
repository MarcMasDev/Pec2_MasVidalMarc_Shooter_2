using UnityEngine;
using UnityEngine.Audio;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject keyVisuals;
    public string uiKeyText;
    public bool grabbed = false;
    [SerializeField] private AudioSource audioSource;
    public bool Interact(GameObject user)
    {
        grabbed = true;
        GetComponent<Collider>().enabled = false;
        keyVisuals.SetActive(false);
        if (audioSource != null) audioSource.Play();
        return true;
    }

    public string GetInteractionText() => uiKeyText;
}