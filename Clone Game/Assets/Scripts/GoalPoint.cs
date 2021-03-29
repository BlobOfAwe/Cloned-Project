using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the goalPoint GameObject
/// </summary>
public class GoalPoint : MonoBehaviour
{
    private Animator animator;
    private AudioPlayer audioPlayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioPlayer>();
    }
    
    // When the player reaches the goal...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        animator.SetTrigger("contact_trig"); // Set the animator trigger. This should play the "level finished" animation
        audioPlayer.PlayAudio(); // Play the attatched audio clip.
    }

    // Loads the next assigned scene. Should only be called by an animation event at the end of the triggered animation.
    public void LoadNextScene()
    {
        // While this is happening, load the next scene and replace this one when ready.
        SceneManager.LoadSceneAsync(GameManager.sceneIndex + 1);
    }
}
