using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioClip jump;
    public AudioClip scoreHighlight;
    private AudioSource audioPlayer;

    void Start(){
        audioPlayer = transform.GetComponent<AudioSource>();
    }

    public void PlayScoreHighlight() {
        audioPlayer.PlayOneShot(scoreHighlight);
    }

    public void PlayJump() {
        audioPlayer.PlayOneShot(jump);
    }
}
