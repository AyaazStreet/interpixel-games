using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    private PostProcessVolume ppv;
    private ColorGrading cg;

    private float sat1 = -50f;
    private float sat2 = 100f;
    private float sat3 = -100f;

    private float changeSpeed = 400f;
    
    void Awake()
    {
        ppv = GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out cg);
    }

    void Update()
    {
        if (TimeScaleManager.globalTimescale == TimeScaleManager.A1_EFFECT)
        {
            if (cg.saturation.value > sat1 + Time.deltaTime * changeSpeed)
            {
                cg.saturation.value -= Time.deltaTime * changeSpeed;
            }
        }
        else if (TimeScaleManager.globalTimescale == TimeScaleManager.A2_EFFECT)
        {
            if (cg.saturation.value < sat2 - Time.deltaTime * changeSpeed)
            {
                cg.saturation.value += Time.deltaTime * changeSpeed;
            }
        }
        else if (TimeScaleManager.globalTimescale == TimeScaleManager.A3_EFFECT)
        {
            if (cg.saturation.value > sat3 + Time.deltaTime * changeSpeed)
            {
                cg.saturation.value -= Time.deltaTime * changeSpeed;
            }
        }
        else
        {
            if (cg.saturation.value > 0 + Time.deltaTime * changeSpeed)
            {
                cg.saturation.value -= Time.deltaTime * changeSpeed;
            }
            else if (cg.saturation.value < 0 - Time.deltaTime * changeSpeed)
            {
                cg.saturation.value += Time.deltaTime * changeSpeed;
            }
            else
            {
                cg.saturation.value = 0;
            }
        }
    }
}
