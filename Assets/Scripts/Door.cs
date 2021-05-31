using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;

    private int openID;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        openID = Animator.StringToHash("Open");
        //register Door
        GameManager.RegisterDoor(this);
    }

    public void Open()
    {
        anim.SetTrigger(openID);
        // play audio
        AudioManager.PlayDoorOpenAudio();
    }
}
