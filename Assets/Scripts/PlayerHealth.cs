using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //
    public GameObject deathVFXPrefab;
    
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
            
            gameObject.SetActive(false);
            
            AudioManager.PlayDeathAudio();
            // restart current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
