using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip soundBlockChange;

    AudioSource myAudioSource;

    public static SoundManager instance; // 정적할당으로 선언

    private void Awake()
    {
        if (SoundManager.instance == null)
            SoundManager.instance = this;
    }

    public void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        // 사운드 한번 재생 시킨다.
        myAudioSource.PlayOneShot(soundBlockChange);
    }
}
