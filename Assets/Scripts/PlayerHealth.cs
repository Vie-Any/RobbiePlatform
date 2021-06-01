using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    //
    public GameObject deathVFXPrefab;
    // death posiotion
    public GameObject deathPos;
    
    // layer index of traps
    private int trapsLayer;
    // Start is called before the first frame update
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == trapsLayer)
        {
            // play death 
            Instantiate(deathVFXPrefab, transform.position, transform.rotation);
            Instantiate(deathPos, transform.position, Quaternion.Euler(0,0,Random.Range(-45,90)));
            
            gameObject.SetActive(false);
            
            AudioManager.PlayDeathAudio();
            // restart current scene
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.PlayerDied();
        }
    }
}
