using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOnAnyButton : MonoBehaviour
{
    public MenuManager menuManager;

    [SerializeField] private bool started;

    private void OnEnable()
    {
        started = false;
    }

    private void OnDisable()
    {
        started = true;
    }

    void Update()
    {
        if (Input.anyKey && !started)
        {
            menuManager.StartButton();
            started = true;
        }
    }
}
