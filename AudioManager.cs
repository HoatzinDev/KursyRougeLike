using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [Header("---------- Audio Clip ---------")]
    public AudioClip background;//music
    public AudioClip PDeath;
    public AudioClip EDeath;
    public AudioClip jump;
    public AudioClip shoot;
    public AudioClip BoxDestroy;
    public AudioClip eat;
    public AudioClip PickUp;
    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
