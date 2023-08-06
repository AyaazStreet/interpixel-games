using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public static float globalTimescale;

    public const float A1_EFFECT = 0.25f;
    public const float A2_EFFECT = 4f;
    public const float A3_EFFECT = 0f;

    void Awake()
    {
        globalTimescale = 1f;
    }
}
