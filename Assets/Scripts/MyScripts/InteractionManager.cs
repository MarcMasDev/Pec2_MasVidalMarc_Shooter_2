using UnityEngine;
public interface IInteractable
{
    void Interact();
    string GetInteractionText();
}
public class InteractionManager : MonoBehaviour
{
    private IEntityInput input;
    private IInteractable currentInteractable;
    [SerializeField] private InteractUI interactUI;

    private void Awake()
    {
        input = GetComponent<IEntityInput>();
    }

    private void Update()
    {
        if (currentInteractable != null)
        {
            interactUI.Show(currentInteractable.GetInteractionText());

            if (input.IsInteracting)
            {
                interactUI.Show(""); //hide
                currentInteractable.Interact();
                currentInteractable = null;
            }
        }
        else interactUI.Show(""); //hide

    }

    private void OnTriggerEnter(Collider other)
    {
        currentInteractable = other.GetComponent<IInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}
