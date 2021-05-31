using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // singleton object reference
    public static GameManager instance;

    // Scene fader
    private SceneFader fader;

    // orb list for registed orb
    private List<Orb> orbs;

    // the door
    private Door lockedDoor;
    
    //
    public float gameTime;

    // orb number of current scene
    // public int orbNum;

    // count player death times
    public int deathCount;

    // game over mark
    private bool gameIsOver;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        orbs = new List<Orb>();
        
        DontDestroyOnLoad(this);
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        instance.fader = obj;
    }

    private void Update()
    {
        if (gameIsOver)
        {
            return;
        }
        // orbNum = instance.orbs.Count;
        UIManager.UpdateOrbGUI(orbs.Count);
        gameTime += Time.deltaTime;
        // refresh display of game time 
        UIManager.UpdateGameTime(gameTime);
    }

    public static void RegisterDoor(Door door)
    {
        instance.lockedDoor = door;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (instance == null)
        {
            return;
        }
        if (!instance.orbs.Contains(orb))
        {
            instance.orbs.Add(orb);
        }
        
        UIManager.UpdateOrbGUI(instance.orbs.Count);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
        {
            return;
        }
        instance.orbs.Remove(orb);

        if (instance.orbs.Count == 0)
        {
            instance.lockedDoor.Open();
        }
        
        UIManager.UpdateOrbGUI(instance.orbs.Count);
    }

    public static void PlayerWin()
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameIsOver();
    }

    public static bool GameOver()
    {
        return instance.gameIsOver;
    }

    public static void PlayerDied()
    {
        instance.fader.FadeOut();
        instance.deathCount++;
        UIManager.UpdateDeathCount(instance.deathCount);
        instance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
