using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Canvas nav;
    public Canvas options;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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

    public void CloseGame()
    {
        Application.Quit();
    }
}
