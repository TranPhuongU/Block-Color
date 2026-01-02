using UnityEngine;
using UnityEngine.UI;

public class BlurRawImage : MonoBehaviour
{
    public RawImage rawImage; // Assign in Inspector
    public Material blurMaterial; // Assign in Inspector
    private Material originalMaterial; // To store the original material

    void Start()
    {
        if (rawImage != null)
        {
            // Store the original material to revert back later
            originalMaterial = rawImage.material;
        }
    }

    public void ApplyBlur()
    {
        if (rawImage != null && blurMaterial != null)
        {
            rawImage.material = blurMaterial;
        }
    }

    public void RemoveBlur()
    {
        if (rawImage != null && originalMaterial != null)
        {
            rawImage.material = originalMaterial;
        }
    }

    // Optional: Method to update blur intensity if using a slider
    public void UpdateBlurIntensity(float intensity)
    {
        if (blurMaterial != null)
        {
            blurMaterial.SetFloat("_BlurSize", intensity); // Ensure your shader has this property
        }
    }
}