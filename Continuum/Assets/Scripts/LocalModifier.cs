using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalModifier : MonoBehaviour
{
    public float? value;

    void Awake()
    {
        value = null;
    }
}
