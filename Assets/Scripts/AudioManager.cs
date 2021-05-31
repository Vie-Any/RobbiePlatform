using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    [Header("Environment Audio")]
    // environment audio
    public AudioClip ambientClip;
    // background music
    public AudioClip musicClip;

    [Header("FX Audio")] 
    // audio of death FX
    public AudioClip deathFXClip;
    // audio of orb explosion
    public AudioClip orbFXClip;
    // audio of door open
    public AudioClip doorFXClip;

    [Header("Robbie Audio")]
    // Robbie walk audio array
    public AudioClip[] walkStepClips;
    // Robbie crouch audio array
    public AudioClip[] crouchStepClips;
    // Robbie jump audio
    public AudioClip jumpClip;
    // Robbie death audio
    public AudioClip deathClip;
    // Robbie jump voice audio
    public AudioClip jumpVoiceClip;
    // Robbie death voice audio
    public AudioClip deathVoiceClip;
    // Robbie collect orb voice audio
    public AudioClip orbVoiceClip;

    //Audio source
    // environment audio source
    private AudioSource ambientSource;
    // background source
    private AudioSource musicSource;
    private AudioSource fxSource;
    private AudioSource playerSource;
    private AudioSource voiceSource;
    
    private void Awake()
    {
        if (null != current)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);
        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();
        // start level audio        
        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        // environment audio
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();
        // background music
        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }

    public static void PlayDoorOpenAudio()
    {
        current.fxSource.clip = current.doorFXClip;
        current.fxSource.PlayDelayed(1f);
    }

    /// <summary>
    /// play the player's walk step audio
    /// </summary>
    public static void PlayFootstepAudio()
    {
        // generate a random index from walk audio array
        int index = Random.Range(0, current.walkStepClips.Length);
        // set audio clip
        current.playerSource.clip = current.walkStepClips[index];
        // play the audio
        current.playerSource.Play();
    }
    /// <summary>
    /// play the player's step audio when the player is crouched
    /// </summary>
    public static void PlayCrouchFootstepAudio()
    {
        // generate a random index from walk audio array
        int index = Random.Range(0, current.crouchStepClips.Length);
        // set audio clip
        current.playerSource.clip = current.crouchStepClips[index];
        // play the audio
        current.playerSource.Play();
    }

    /// <summary>
    /// play the jump audio when the player jump
    /// </summary>
    public static void PlayJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();
        current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }

    /// <summary>
    /// player death audio
    /// </summary>
    public static void PlayDeathAudio()
    {
        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();
        
        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();
        
        current.fxSource.clip = current.deathFXClip;
        current.fxSource.Play();
    }

    /// <summary>
    /// player collect orb audio
    /// </summary>
    public static void PlayOrbAudio()
    {
        current.fxSource.clip = current.orbFXClip;
        current.fxSource.Play();
        
        current.voiceSource.clip = current.orbVoiceClip;
        current.voiceSource.Play();
    }

}
