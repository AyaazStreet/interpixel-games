using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (GameManager.Instance.currLevel + 1)
            {
                case 0:
                    SceneManager.LoadScene("Level_Tutorial");
                    break;
                case 1:
                    SceneManager.LoadScene("Level_1");
                    break;
                case 2:
                    SceneManager.LoadScene("Level_2");
                    break;
                case 3:
                    SceneManager.LoadScene("Level_3");
                    break;
                case 4:
                    SceneManager.LoadScene("Level_4");
                    break;
                case 5:
                    SceneManager.LoadScene("Level_5");
                    break;
                default:
                    SceneManager.LoadScene("Menu_Main");
                    break;
            }
        }
    }
}
