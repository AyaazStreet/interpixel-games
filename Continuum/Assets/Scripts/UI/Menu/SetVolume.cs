using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public TextMeshProUGUI txt;

    public void SetLevel(float sliderVal)
    {
        mixer.SetFloat("SFX_Vol", Mathf.Log10(sliderVal) * 20);
        
        if (txt != null )
        {
            txt.text = (Mathf.Round(sliderVal * 100.0f)).ToString();
        }
    }
}
