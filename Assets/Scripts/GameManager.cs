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

    // orb number of current scene
    public int orbNum;

    // count player death times
    public int deathCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            orbs = new List<Orb>();
            
            DontDestroyOnLoad(this);
        }
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        instance.fader = obj;
    }

    private void Update()
    {
        orbNum = instance.orbs.Count;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
        {
            instance.orbs.Add(orb);
        }
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
        {
            return;
        }
        instance.orbs.Remove(orb);
    }

    public static void PlayerDied()
    {
        instance.fader.FadeOut();
        instance.deathCount++;
        instance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
