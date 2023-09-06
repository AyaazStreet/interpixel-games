using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    private float sat1 = 0.5f;
    private float sat2 = 2f;
    private float sat3 = 0f;

    private float changeSpeed = 4f;

    public Material area;
    public Material sprite;
    
    void Awake()
    {
        Debug.Log(area.GetFloat("_Saturation"));
    }

    void Update()
    {
        if (TimeScaleManager.globalTimescale == TimeScaleManager.A1_EFFECT)
        {
            if (area.GetFloat("_Saturation") > sat1 + Time.deltaTime * changeSpeed)
            {
                area.SetFloat("_Saturation", area.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
                sprite.SetFloat("_Saturation", sprite.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
            }
        }
        else if (TimeScaleManager.globalTimescale == TimeScaleManager.A2_EFFECT)
        {
            if (area.GetFloat("_Saturation") < sat2 - Time.deltaTime * changeSpeed)
            {
                area.SetFloat("_Saturation", area.GetFloat("_Saturation") + Time.deltaTime * changeSpeed);
                sprite.SetFloat("_Saturation", sprite.GetFloat("_Saturation") + Time.deltaTime * changeSpeed);
            }
        }
        else if (TimeScaleManager.globalTimescale == TimeScaleManager.A3_EFFECT)
        {
            if (area.GetFloat("_Saturation") > sat3 + Time.deltaTime * changeSpeed)
            {
                area.SetFloat("_Saturation", area.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
                sprite.SetFloat("_Saturation", sprite.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
            }
        }
        else
        {
            if (area.GetFloat("_Saturation") > 1 + Time.deltaTime * changeSpeed)
            {
                area.SetFloat("_Saturation", area.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
                sprite.SetFloat("_Saturation", sprite.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
            }
            else if (area.GetFloat("_Saturation") < 1 - Time.deltaTime * changeSpeed)
            {
                area.SetFloat("_Saturation", area.GetFloat("_Saturation") + Time.deltaTime * changeSpeed);
                sprite.SetFloat("_Saturation", sprite.GetFloat("_Saturation") + Time.deltaTime * changeSpeed);
            }
            else
            {
                area.SetFloat("_Saturation", 1);
                sprite.SetFloat("_Saturation", 1);
            }
        }
    }
}
