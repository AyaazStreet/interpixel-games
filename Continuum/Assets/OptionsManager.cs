using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetTogglePowers (bool togglePowers)
    {
        CheckpointManager.Instance.togglePowers = togglePowers;
    }
}
