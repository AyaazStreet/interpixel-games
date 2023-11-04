using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeScoreManager : MonoBehaviour
{
    public static TimeScoreManager Instance { get; private set; }

    public bool running;
    public float[] times = new float[11];
    public float[] parTimes = new float[11];

    public int selectedBest;

    float currentTime;
    int currLevel;
    TextMeshProUGUI timerText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            running = false;
            Array.Fill(times, float.PositiveInfinity);

            parTimes = new float[] {
                0,
                32,
                40,
                20,
                16,
                35,
                50,
                42,
                55,
                18,
                38,
            };
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (running)
        {
            currentTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartTimer()
    {
        if (GameManager.Instance != null)
        {
            currLevel = GameManager.Instance.currLevel;
            timerText = GameManager.Instance.gameTimer;

            if (!running)
            {
                running = true;
                currentTime = 0;
            }
        }
    }

    public void RestartTimer()
    {
        if (GameManager.Instance != null)
        {
            currLevel = GameManager.Instance.currLevel;
            timerText = GameManager.Instance.gameTimer;

            if (running)
            {
                running = true;
                currentTime = 0;
            }
        }
    }

    public void StopTimer()
    {
        if (running)
        {
            running = false;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            

            if (currentTime < parTimes[currLevel]) 
            {
                GameManager.Instance.pc.DisplayPopup("" + string.Format("{0:00}:{1:00}", minutes, seconds), Color.green);
            }
            else
            {
                GameManager.Instance.pc.DisplayPopup("" + string.Format("{0:00}:{1:00}", minutes, seconds));
            }

            times[currLevel] = currentTime;

            currentTime = 0;
        }
    }

    public void CancelTimer()
    {
        if (running)
        {
            running = false;
            currentTime = 0;
        }
    }

    public void EraseTimes()
    {
        Array.Fill(times, float.PositiveInfinity);
    }
}
