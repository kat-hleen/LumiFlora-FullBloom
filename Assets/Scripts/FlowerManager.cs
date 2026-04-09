using UnityEngine;
using System.Collections.Generic;

public class FlowerManager : MonoBehaviour
{
    [Header("Player / Distance Settings")]
    public Transform playerCamera;       // assign your camera or player object
    public float triggerDistance = 3f;   // distance to bloom
    public float closeBuffer = 1f;       // extra distance to prevent flicker

    [Header("Light Settings")]
    public float lightMaxIntensity = 3f; // max bloom light
    public float lightFadeSpeed = 2f;    // how fast light fades in/out

    private List<Flower> flowers = new List<Flower>();

    void Start()
    {
        // Find all Flower scripts in the scene (new API)
        Flower[] allFlowers = Object.FindObjectsByType<Flower>(FindObjectsSortMode.None);
        flowers.AddRange(allFlowers);
    }

    void Update()
    {
        foreach (var flower in flowers)
        {
            float distance = Vector3.Distance(playerCamera.position, flower.transform.position);

            // Trigger bloom when close
            if (!flower.hasBloomed && distance < triggerDistance)
            {
                Debug.Log("Bloom triggered for " + flower.name + " | Distance: " + distance);

                flower.animator.SetTrigger("BloomTrigger");
                flower.hasBloomed = true;
            }
            // Trigger close when far
            else if (flower.hasBloomed && distance > triggerDistance + closeBuffer)
            {
                flower.animator.SetTrigger("CloseTrigger");
                flower.hasBloomed = false;
            }

            // Smoothly update light intensity
            float targetIntensity = (flower.hasBloomed && distance < triggerDistance) ? lightMaxIntensity : 0f;
            flower.UpdateLight(targetIntensity, lightFadeSpeed);
        }
    }
}