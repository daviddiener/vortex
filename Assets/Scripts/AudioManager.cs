using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource shieldOffSound;
    [SerializeField] private AudioSource itemPickup;
    [SerializeField] private AudioSource gameStart;
    [SerializeField] private AudioSource popSound;

    public void playClickSound(){
        clickSound.Play();
    }

    public void playDamageSound(){
        damageSound.Play();
    }

    public void playShieldOffSound(){
        shieldOffSound.Play();
    }
    
    public void playItemPickup(){
        itemPickup.Play();
    }

    public void playGameStart(){
        gameStart.Play();
    }

    public void playPopSound(){
        popSound.Play();
    }
    
}
