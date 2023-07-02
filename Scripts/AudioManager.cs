using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetAndPlayAudio(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.PlayOneShot(audioSource.clip);
    }
}
