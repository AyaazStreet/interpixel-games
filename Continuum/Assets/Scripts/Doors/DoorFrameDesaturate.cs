using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DoorFrameDesaturate : MonoBehaviour
{
    SpriteRenderer sr;

    private float sat1 = 0.5f;
    private float sat2 = -1f;
    private float sat3 = 1f;

    private float changeSpeed = 4f;

    [SerializeField] private float val;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        val = sr.material.GetFloat("_GrayscaleAmount");
    }

    void Update()
    {
        val = sr.material.GetFloat("_GrayscaleAmount");

        if (TimeScaleManager.globalTimescale == TimeScaleManager.A1_EFFECT)
        {
            if (val < sat1 - Time.deltaTime * changeSpeed)
            {
                sr.material.SetFloat("_GrayscaleAmount", val + Time.deltaTime * changeSpeed);
            }
        }
        else if (TimeScaleManager.globalTimescale == TimeScaleManager.A2_EFFECT)
        {
            if (val > sat2 + Time.deltaTime * changeSpeed)
            {
                sr.material.SetFloat("_GrayscaleAmount", val - Time.deltaTime * changeSpeed);
            }
        }
        else if (TimeScaleManager.globalTimescale == TimeScaleManager.A3_EFFECT)
        {
            if (val < sat3 - Time.deltaTime * changeSpeed)
            {
                sr.material.SetFloat("_GrayscaleAmount", val + Time.deltaTime * changeSpeed);
            }
        }
        else
        {
            if (val > 0 + Time.deltaTime * changeSpeed)
            {
                sr.material.SetFloat("_GrayscaleAmount", val - Time.deltaTime * changeSpeed);
            }
            else if (val < 0 - Time.deltaTime * changeSpeed)
            {
                sr.material.SetFloat("_GrayscaleAmount", val + Time.deltaTime * changeSpeed);
            }
            else
            {
                sr.material.SetFloat("_GrayscaleAmount", 0);
            }
        }
    }
}
