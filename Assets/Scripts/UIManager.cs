using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // singleton object reference
    public static UIManager instance;

    // text for display orb collected
    public TextMeshProUGUI orbText;
    
    // text for time spended
    public TextMeshProUGUI timeText;
    
    // text for player dead
    public TextMeshProUGUI deathText;
    
    // text for game over
    public TextMeshProUGUI gameOverText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void UpdateOrbGUI(int orbCount)
    {
        instance.orbText.text = orbCount.ToString();
    }

    public static void UpdateDeathCount(int deathCount)
    {
        instance.deathText.text = deathCount.ToString();
    }
    
    public static void UpdateGameTime(float gameTime)
    {

        int minutes = (int) (gameTime / 60);
        float seconds = gameTime % 60;
        
        instance.timeText.text = minutes.ToString("00")+":"+seconds.ToString("00");
    }

    public static void DisplayGameIsOver()
    {
        instance.gameOverText.enabled = true;
    }
}
