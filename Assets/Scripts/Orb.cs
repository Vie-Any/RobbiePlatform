using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    
    // layer index of the player
    private int player;

    // VFX of orb explosion
    public GameObject explosionVFXPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        player = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == player)
        {
            Instantiate(explosionVFXPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayOrbAudio();
        }
    }
}
