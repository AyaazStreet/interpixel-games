using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set;}

    public GameObject pauseUI;

    public GameObject optionsUI;

    public Texture2D cursor;
    public Texture2D aimCursor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        optionsUI.transform.Find("tgl_PowerSetting").GetComponent<Toggle>().isOn = CheckpointManager.Instance.togglePowers;
        optionsUI.transform.Find("tgl_Fullscreen").GetComponent<Toggle>().isOn = Screen.fullScreen;

        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
    }

    public void TogglePause()
    {
        if(pauseUI != null)
        {
            SoundManager.PlaySound(SoundManager.Sound.snd_click);

            if (pauseUI.activeSelf)
            {
                UnityEngine.Cursor.SetCursor(aimCursor, new Vector2(aimCursor.width / 2, aimCursor.height / 2), CursorMode.Auto);

                pauseUI.SetActive(false);
                Time.timeScale = 1f;
            }
            else if (optionsUI.activeSelf)
            {
                CloseOptions();
            }
            else
            {
                UnityEngine.Cursor.SetCursor(cursor, new Vector2(0, 0), CursorMode.Auto);

                pauseUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else
        {
            Debug.LogError("No pause ui found");
        }
    }

    public void Save()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_click);

        SaveManager.SaveData(GameManager.Instance.pc, GameManager.Instance.currLevel);
    }

    public void Options()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_click);

        optionsUI.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void CloseOptions()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_click);

        optionsUI.SetActive(false);
        pauseUI.SetActive(true);
    }

    public void QuitToMenu()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_click);

        SceneManager.LoadScene("Menu_Main");
    }

    public void QuitToDesktop()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_click);

        Application.Quit();
    }
}
