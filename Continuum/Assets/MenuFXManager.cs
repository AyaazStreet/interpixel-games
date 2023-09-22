using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFXManager : MonoBehaviour
{
    public Material fullscreen;
    
    // Start is called before the first frame update
    void Start()
    {
        if (fullscreen.GetFloat("_FullscreenIntesity") != 0)
        {
            fullscreen.SetFloat("_FullscreenIntesity", 0);
        }

    }
}
