using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    private float sat1 = 0.5f;
    private float sat2 = 2f;
    private float sat3 = 0f;

    private float changeSpeed = 4f;

    private float radiusMax = 0.7f;
    private float radiusChange = 0.5f;

    public Material area;
    public Material sprite;

    public Material fullscreen;
    
    void Awake()
    {
        
    }

    void Update()
    {
        if (TimeScaleManager.globalTimescale == TimeScaleManager.A1_EFFECT)
        {
            //Saturation
            if (area.GetFloat("_Saturation") > sat1 + Time.deltaTime * changeSpeed)
            {
                area.SetFloat("_Saturation", area.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
                sprite.SetFloat("_Saturation", sprite.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
            }

            //Vignette
            if (fullscreen.GetColor("_Color") != TimeScaleManager.A1_COLOR)
            {
                fullscreen.SetColor("_Color", TimeScaleManager.A1_COLOR);
            }
            if (fullscreen.GetFloat("_Radius") < radiusMax)
            {
                fullscreen.SetFloat("_Radius", fullscreen.GetFloat("_Radius") + Time.deltaTime * radiusChange);
            }
            if (fullscreen.GetFloat("_FullscreenIntesity") != 1)
            {
                fullscreen.SetFloat("_FullscreenIntesity", 1);
            }
        }
        else if (TimeScaleManager.globalTimescale == TimeScaleManager.A2_EFFECT)
        {
            //Saturation
            if (area.GetFloat("_Saturation") < sat2 - Time.deltaTime * changeSpeed)
            {
                area.SetFloat("_Saturation", area.GetFloat("_Saturation") + Time.deltaTime * changeSpeed);
                sprite.SetFloat("_Saturation", sprite.GetFloat("_Saturation") + Time.deltaTime * changeSpeed);
            }

            //Vignette
            if (fullscreen.GetColor("_Color") != TimeScaleManager.A2_COLOR)
            {
                fullscreen.SetColor("_Color", TimeScaleManager.A2_COLOR);
            }
            if (fullscreen.GetFloat("_Radius") < radiusMax)
            {
                fullscreen.SetFloat("_Radius", fullscreen.GetFloat("_Radius") + Time.deltaTime * radiusChange);
            }
            if (fullscreen.GetFloat("_FullscreenIntesity") != 1)
            {
                fullscreen.SetFloat("_FullscreenIntesity", 1);
            }
        }
        else if (TimeScaleManager.globalTimescale == TimeScaleManager.A3_EFFECT)
        {
            //Saturation
            if (area.GetFloat("_Saturation") > sat3 + Time.deltaTime * changeSpeed)
            {
                area.SetFloat("_Saturation", area.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
                sprite.SetFloat("_Saturation", sprite.GetFloat("_Saturation") - Time.deltaTime * changeSpeed);
            }

            //Vignette
            if (fullscreen.GetColor("_Color") != TimeScaleManager.A3_COLOR)
            {
                fullscreen.SetColor("_Color", TimeScaleManager.A3_COLOR);
            }
            if (fullscreen.GetFloat("_Radius") < radiusMax)
            {
                fullscreen.SetFloat("_Radius", fullscreen.GetFloat("_Radius") + Time.deltaTime * radiusChange);
            }
            if (fullscreen.GetFloat("_FullscreenIntesity") != 1)
            {
                fullscreen.SetFloat("_FullscreenIntesity", 1);
            }
        }
        else
        {
            //Saturation
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

            //Vignette
            
            if (fullscreen.GetFloat("_Radius") > 0.5f + Time.deltaTime * radiusChange)
            {
                fullscreen.SetFloat("_Radius", fullscreen.GetFloat("_Radius") - Time.deltaTime * radiusChange);
            }
            else if(fullscreen.GetFloat("_FullscreenIntesity") != 0)
            {
                fullscreen.SetFloat("_FullscreenIntesity", 0);
            }
        }
    }
}
