using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This Class allows any other program attatched to the same gameObject to play an attatched AudioSource.
///  On it's own, this code does nothing.
/// </summary>
public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Can be called from another function on the same gameObject in order to play the attatched AudioSource
    public void PlayAudio()
    {
        audioSource.Play();
    }
}
