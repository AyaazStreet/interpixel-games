using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GodManager : MonoBehaviour
{
    public static GodManager Instance { get; private set;}

    public GameObject godUI;

    public Toggle invincibility;
    public Toggle slow;
    public Toggle accel;
    public Toggle stop;
    public Button bolt;
    public Button can;
    public Button rod;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        godUI.SetActive(false);
    }

    public void ToggleGod()
    {
        if(godUI != null)
        {
            //SoundManager.PlaySoundPersistent(SoundManager.Sound.snd_click);

            if (godUI.activeSelf)
            {
                godUI.SetActive(false);

                GameManager.Instance.pc.invincible = false;
            }
            else
            {
                godUI.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("No god ui found");
        }
    }

    public void Invincibility(bool val)
    {
        GameManager.Instance.pc.invincible = val;
    }

    public void Slow(bool val)
    {
        GameManager.Instance.pc.A1_Unlocked = val;
    }

    public void Accel(bool val)
    {
        GameManager.Instance.pc.A2_Unlocked = val;
    }

    public void Stop(bool val)
    {
        GameManager.Instance.pc.A3_Unlocked = val;
    }

    public void Bolt()
    {
        GameManager.Instance.em.E1_count++;
    }

    public void Can()
    {
        GameManager.Instance.em.E2_count++;
    }

    public void Rod()
    {
        GameManager.Instance.em.E3_count++;
    }
}
