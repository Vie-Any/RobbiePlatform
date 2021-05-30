using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManage : MonoBehaviour
{
    public static AudioManage current;

    [Header("Environment Audio")]
    // environment background audio
    public AudioClip ambientClip;
    // background music audio
    public AudioClip musicClip;

    [Header("FX Audio")]
    // death FX audio 
    public AudioClip deathFXClip;

    [Header("Robbie Audio")]
    //movement audio when the player was stand
    public AudioClip[] walkStepClips;
    // movement audio when the player was crouched
    public AudioClip[] crouchStepClips;
    // jump audio from Robbie footsteps
    public AudioClip jumpClip;
    // death audio of the player
    public AudioClip deathClip;
    
    // jump audio from Robbie
    public AudioClip jumpVoiceClip;
    // death audio of the player's voice
    public AudioClip deathVoiceClip;

    [Header("Audio Source")]
    // audio source for background of environment audio
    private AudioSource ambientSource;
    // audio source for background music audio
    private AudioSource musicSource;
    // audio source for fx
    private AudioSource fxSource;
    // audio source for the player
    private AudioSource playerSource;
    // audio source for the player's voice
    private AudioSource voiceSource;

    private void Awake()
    {
        // Init singleton object
        current = this;
        // make AudioManage lifecycle same as the game process even change current scene to the next one. 
        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();
        
        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        // environment background audio
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();
        
        // background music audio
        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();
        
    }

    /// <summary>
    /// play footstep audio for the player movement
    /// </summary>
    public static void PlayFootstepAudio()
    {
        // get a random index from walkStepClips array
        int index = Random.Range(0, current.walkStepClips.Length);

        current.playerSource.clip = current.walkStepClips[index];
        current.playerSource.Play();
    }
    
    /// <summary>
    /// play footstep audio for the player movement in crouch state
    /// </summary>
    public static void PlayCrouchFootstepAudio()
    {
        // get a random index from walkStepClips array
        int index = Random.Range(0, current.crouchStepClips.Length);

        current.playerSource.clip = current.crouchStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();
        
        current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }

    public static void PlayDeathAudio()
    {
        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();

        current.fxSource.clip = current.deathFXClip;
        current.fxSource.Play();
    }
}
