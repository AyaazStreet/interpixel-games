using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public static float globalTimescale;

    public const float A1_EFFECT = 0.25f;
    public const float A2_EFFECT = 4f;
    public const float A3_EFFECT = 0f;

    public static Color A1_COLOR = new(0f, 1f, 0.851f); //Teal
    public static Color A2_COLOR = new(1f, 0.4f, 0f); //Orange
    public static Color A3_COLOR = new(0.553f, 0f, 1f); //Purple

    void Awake()
    {
        globalTimescale = 1f;
    }
}
