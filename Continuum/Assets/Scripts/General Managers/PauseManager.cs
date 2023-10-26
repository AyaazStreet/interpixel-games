using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set;}

    public InputActionAsset inputs;

    public GameObject pauseUI;

    public GameObject optionsUI;

    public Texture2D cursor;
    public Texture2D aimCursor;

    [Header("Buttons")]
    [SerializeField] private Button startSelect;
    [SerializeField] private Toggle optionsSelect;

    private void OnEnable()
    {
        inputs.FindAction("Restart").performed += Back_performed;
    }

    private void OnDisable()
    {
        inputs.FindAction("Restart").performed += Back_performed;
    }

    public void Back_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if ((pauseUI && pauseUI.activeSelf) || (optionsUI && optionsUI.activeSelf))
            {
                TogglePause();
            }
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
    }

    public void TogglePause()
    {
        if(pauseUI != null)
        {
            SoundManager.PlaySoundPersistent(SoundManager.Sound.snd_click);

            if (pauseUI.activeSelf)
            {
                Cursor.SetCursor(aimCursor, new Vector2(aimCursor.width / 2, aimCursor.height / 2), CursorMode.Auto);

                pauseUI.SetActive(false);
                Time.timeScale = 1f;
            }
            else if (optionsUI.activeSelf)
            {
                CloseOptions();
                
            }
            else if (GameManager.Instance.pc.alive)
            {
                Cursor.SetCursor(cursor, new Vector2(0, 0), CursorMode.Auto);

                pauseUI.SetActive(true);
                startSelect.Select();
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
        SoundManager.PlaySoundPersistent(SoundManager.Sound.snd_click);

        SaveManager.SaveData(GameManager.Instance.pc, GameManager.Instance.currLevel);
    }

    public void Options()
    {
        SoundManager.PlaySoundPersistent(SoundManager.Sound.snd_click);

        optionsUI.transform.Find("tgl_PowerSetting").GetComponent<Toggle>().isOn = CheckpointManager.Instance.togglePowers;
        optionsUI.transform.Find("tgl_Fullscreen").GetComponent<Toggle>().isOn = Screen.fullScreen;

        optionsUI.SetActive(true);
        pauseUI.SetActive(false);

        optionsSelect.Select();
    }

    public void CloseOptions()
    {
        SoundManager.PlaySoundPersistent(SoundManager.Sound.snd_click);

        optionsUI.SetActive(false);
        pauseUI.SetActive(true);

        startSelect.Select();
    }

    public void RestartLevel()
    {
        CheckpointManager.Instance.ResetCheckpointData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        SoundManager.PlaySoundPersistent(SoundManager.Sound.snd_click);

        CheckpointManager.Instance.skipIntro = true;
        SceneManager.LoadScene("Menu_Main");
    }

    public void QuitToDesktop()
    {
        SoundManager.PlaySoundPersistent(SoundManager.Sound.snd_click);

        Application.Quit();
    }
}
