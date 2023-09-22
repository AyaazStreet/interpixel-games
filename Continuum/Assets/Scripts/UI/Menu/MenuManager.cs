using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button start;
    public Canvas nav;
    public Canvas levelSelect;
    public Canvas options;
    public Canvas keybinds;
    public Canvas keybindsK;
    public Canvas keybindsG;

    public float timerMax = 1f;
    public float timer = 0f;
    bool started = false;

    private CheckpointManager checkpointManager;
    public GameObject cmPrefab;

    public Animator backgroundAnim;
    public Animator titleAnim;

    private void Awake()
    {
        if (start) start.gameObject.SetActive(true);
        if (nav) nav.gameObject.SetActive(false);
        if (levelSelect) levelSelect.gameObject.SetActive(false);
        if (options) options.gameObject.SetActive(false);
        if (keybinds) keybinds.gameObject.SetActive(false);
        if (keybindsK) keybindsK.gameObject.SetActive(false);
        if (keybindsG) keybindsG.gameObject.SetActive(false);

        PlayerData data = SaveManager.LoadData();

        //Checkpoint Object Create
        if (GameObject.FindGameObjectWithTag("CheckpointManager"))
        {
            checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
        }
        else
        {
            GameObject cm = Instantiate(cmPrefab);
            cm.name = "CheckpointManager";
            checkpointManager = cm.GetComponent<CheckpointManager>();
        }

        //checkpointManager.savedLevel = -1;

        //Data load
        if (data != null)
        {
            //loading checkpoint data
        }
    }

    private void Update()
    {
        if (started)
        {
            if (timer < timerMax)
            {
                timer += Time.deltaTime;
            }
            else
            {
                started = false;
                nav.gameObject.SetActive(true);
            }
        }
    }

    public void StartButton()
    {
        started = true;
        backgroundAnim.SetTrigger("start");
        titleAnim.SetTrigger("start");
        start.gameObject.SetActive(false);
    }

    public void Continue()
    {
        //Level Select
        switch (checkpointManager.savedLevel)
        {
            case 1:
                ChangeScene("Level_1");
                break;
            case 2:
                ChangeScene("Level_2");
                break;
            case 3:
                ChangeScene("Level_3");
                break;
            case 4:
                ChangeScene("Level_4");
                break;
            default:
                ChangeScene("Level_Tutorial");
                break;
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleLevelSelect()
    {
        if (nav.gameObject.activeSelf)
        {
            nav.gameObject.SetActive(false);
            levelSelect.gameObject.SetActive(true);
        }
        else if (levelSelect.gameObject.activeSelf)
        {
            levelSelect.gameObject.SetActive(false);
            nav.gameObject.SetActive(true);
        }
    }

    public void ToggleOptions()
    {
        if (nav.gameObject.activeSelf)
        {
            nav.gameObject.SetActive(false);
            options.gameObject.SetActive(true);
        }
        else if (options.gameObject.activeSelf)
        {
            options.gameObject.SetActive(false);
            nav.gameObject.SetActive(true);
        }
    }

    public void ToggleKeybindings()
    {
        if (options.gameObject.activeSelf)
        {
            options.gameObject.SetActive(false);
            keybinds.gameObject.SetActive(true);
        }
        else if (keybinds.gameObject.activeSelf)
        {
            keybinds.gameObject.SetActive(false);
            options.gameObject.SetActive(true);
        }
    }

    public void ToggleKeyboardKeybindings()
    {
        if (keybinds.gameObject.activeSelf)
        {
            keybinds.gameObject.SetActive(false);
            keybindsK.gameObject.SetActive(true);
        }
        else if (keybindsK.gameObject.activeSelf)
        {
            keybindsK.gameObject.SetActive(false);
            keybinds.gameObject.SetActive(true);
        }
    }

    public void ToggleGamepadKeybindings()
    {
        if (keybinds.gameObject.activeSelf)
        {
            keybinds.gameObject.SetActive(false);
            keybindsG.gameObject.SetActive(true);
        }
        else if (keybindsG.gameObject.activeSelf)
        {
            keybindsG.gameObject.SetActive(false);
            keybinds.gameObject.SetActive(true);
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }


}
