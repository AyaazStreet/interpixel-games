using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set;}

    public GameObject pauseUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        pauseUI = GameObject.Find("PauseScreen");
    }

    public void TogglePause()
    {
        if(pauseUI != null)
        {
            if (pauseUI.activeSelf)
            {
                pauseUI.SetActive(false);
            }
            else
            {
                pauseUI.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("No pause ui found");
        }
    }

    public void Save()
    {
        SaveManager.SaveData(GameManager.Instance.pc);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu_Main");
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
