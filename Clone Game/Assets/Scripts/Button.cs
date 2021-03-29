using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls the button asset (the gameobject, not the UI element)
/// </summary>
public class Button : MonoBehaviour
{
    public UnityEvent buttonFunction; // Allows the button to call an assigned function when pressed
    [SerializeField] bool stayClosed = false; // Asks if the button stays closed after the player leaves, if false, the button can be activated multiple times.
    [SerializeField] Sprite open; // Sprite for when the button is open
    [SerializeField] Sprite closed; // Sprite for when the button is closed
    private bool opened; // records if the button is open
    private AudioSource audio;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        opened = true; // Button should always start as open
        audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the game JUST entered gameState 1 (the state before a level starts) the frame before, set the button to open.
        if (GameManager.gameState == 1 && GameManager.framesSinceStateChange == 1) // This function essentially mirror's Start()
        {
            opened = true;
        }
        
        // Set's the gameObject's sprite based on whether the button is open or not
        if (opened) { spriteRenderer.sprite = open; }
        else { spriteRenderer.sprite = closed; }
    }

    // When the player triggers the button...
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        // If the button is open, close it and play the attatched sound
        if (opened) 
        {
            audio.Play();
            opened = false; buttonFunction.Invoke(); 
        } 
    }
    private void OnTriggerExit2D(Collider2D collision) {if (!stayClosed) { opened = true; }} // If the button is set to reopen, when the player leaves, open the button
}
