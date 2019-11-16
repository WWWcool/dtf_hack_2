using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    private static PauseSystem instance = null;
    private bool isPaused = false;
    private int pausedCount = 0;

    public static PauseSystem Instance
    {
        get
        {
            if (instance == null) Debug.LogError("PauseSystem is Null");
            return instance;
        }
    }
    public bool IsPaused
    {
        get { return isPaused; }
    }
    public int PausedCount
    {
        get { return pausedCount; }
    }

    private void Start()
    {
        if (instance == null) instance = this;
        else if (instance == this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    public void PauseOnOff(bool on)
    {
        if (on)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
            pausedCount++;
        }
        else
        {
            pausedCount--;
            if (pausedCount == 0)
            {
                Time.timeScale = 1.0f;
                isPaused = false;
            }
        }
    }
    public void NeedPause(bool needPause)
    {
        if (needPause)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
            pausedCount++;
        }
        else
        {
            Time.timeScale = 1.0f;
            isPaused = false;
            pausedCount = 0;
        }
    }
}
