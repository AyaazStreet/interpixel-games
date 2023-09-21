using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    public int savedLevel;
    public Vector3 savedPosition;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savedLevel = -1;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
