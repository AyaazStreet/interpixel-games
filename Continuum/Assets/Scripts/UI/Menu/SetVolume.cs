using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public TextMeshProUGUI txt;
    public string group;

    private float previewDuration = 0.2f;
    private float previewTimer = 0f;
    private bool canPlayPreview = false;

    private void Update()
    {
        if (previewTimer > 0f)
        {
            if(!canPlayPreview)
            {
                previewTimer -= Time.deltaTime;
            }
        }
        else
        {
            previewTimer = previewDuration;
            canPlayPreview = true;
        }
    }

    public void SetLevel(float sliderVal)
    {
        mixer.SetFloat(group, Mathf.Log10(sliderVal) * 20);
        
        if (txt != null )
        {
            txt.text = (Mathf.Round(sliderVal * 100.0f)).ToString();
        }

        if (group == "Effects_Vol" || group == "Master_Vol")
        {
            if(canPlayPreview)
            {
                Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                SoundManager.PlaySound(SoundManager.Sound.snd_interact_btn, center);

                canPlayPreview = false;
            }
        }
    }
}
