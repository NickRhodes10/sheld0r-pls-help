using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light torch;
    public float minIntensity = 0.95f;
    public float maxIntensity = 1.05f;
    public float flickerRate = 0.1f;
    private bool flickering;
    private bool canFlicker;

    // Start is called before the first frame update
    void Start()
    {
        torch = GetComponent<Light>();
        flickering = false;
        canFlicker = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canFlicker && !flickering)
        {
            PerformFlicker();
            canFlicker = false;
        }
    }

    public void PerformFlicker()
    {
        flickering = true;
        torch.intensity = Random.Range(minIntensity, maxIntensity);
        Invoke("ResetFlicker", flickerRate);
    }

    public void ResetFlicker()
    {
        canFlicker = true;
        flickering = false;
    }
}
