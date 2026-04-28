using UnityEngine;

public class ThermalTarget : MonoBehaviour
{
    public Material revealMaterial;
    private Material originalMaterial;
    private SkinnedMeshRenderer skinnedRenderer;

    void Awake()
    {
        skinnedRenderer = GetComponent<SkinnedMeshRenderer>();

        if (skinnedRenderer != null)
            originalMaterial = skinnedRenderer.sharedMaterial;
    }

    void OnEnable()
    {
        FlashlightController.OnFlashlight += ShowEffect;
    }

    void OnDisable()
    {
        FlashlightController.OnFlashlight -= ShowEffect;
    }

    void ShowEffect(bool enabled)
    {
        if (enabled) skinnedRenderer.material = new Material(revealMaterial);
        else skinnedRenderer.sharedMaterial = originalMaterial;
    }
}
