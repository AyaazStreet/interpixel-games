using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public void SetFullscreen (bool isFullscreen)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.PlaySound(SoundManager.Sound.snd_click);
        }
        
        Screen.fullScreen = isFullscreen;
    }

    public void SetTogglePowers (bool togglePowers)
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_click);

        CheckpointManager.Instance.togglePowers = togglePowers;
    }
}
