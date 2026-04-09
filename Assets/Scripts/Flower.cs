using UnityEngine;

public class Flower : MonoBehaviour
{
    [HideInInspector] public Animator animator;  // reference to Animator
    [HideInInspector] public Light flowerLight;  // reference to child Light
    [HideInInspector] public bool hasBloomed = false; // track bloom state

    void Awake()
    {
        // Get components
        animator = GetComponent<Animator>();
        flowerLight = GetComponentInChildren<Light>();

        // Start with light off
        if (flowerLight != null)
            flowerLight.intensity = 0f;
    }

    // Smoothly update light intensity
    public void UpdateLight(float targetIntensity, float fadeSpeed)
    {
        if (flowerLight != null)
            flowerLight.intensity = Mathf.Lerp(flowerLight.intensity, targetIntensity, Time.deltaTime * fadeSpeed);
    }
}