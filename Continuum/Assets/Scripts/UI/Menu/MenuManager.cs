using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Canvas nav;
    public Canvas levelSelect;
    public Canvas options;
    public Canvas keybinds;
    public Canvas keybindsK;
    public Canvas keybindsG;

    private void Awake()
    {
        if (nav) nav.gameObject.SetActive(true);
        if (levelSelect) levelSelect.gameObject.SetActive(false);
        if (options) options.gameObject.SetActive(false);
        if (keybinds) keybinds.gameObject.SetActive(false);
        if (keybindsK) keybindsK.gameObject.SetActive(false);
        if (keybindsG) keybindsG.gameObject.SetActive(false);
    }

    public void Continue()
    {
        PlayerData data = SaveManager.LoadData();

        if(data != null)
        {
            Debug.Log("Loading Level" + data.level);
            switch (data.level)
            {
                case 0: 
                    ChangeScene("Level_Tutorial");
                    break;
                case 1: 
                    ChangeScene("Level_1");
                    break; 
                case 2:
                    ChangeScene("Level_2");
                    break;
                case 3:
                    ChangeScene("Level_3");
                    break;
            }
        }
        else
        {
            ChangeScene("Level_Tutorial");
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
