using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject keyVisuals;
    public string uiKeyText;
    public bool grabbed = false;

    public void Interact()
    {
        grabbed = true;
        GetComponent<Collider>().enabled = false;
        keyVisuals.SetActive(false);
    }

    public string GetInteractionText() => uiKeyText;
}