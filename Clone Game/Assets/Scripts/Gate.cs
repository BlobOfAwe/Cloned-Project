using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Gate GameObjects. This should be attatched to the un-moving portion of the gate.
/// </summary>
public class Gate : MonoBehaviour
{
    private Animator animator; // Stores the animator Controller
    private AudioSource audio; // Stores the gameObject's AudioSource
    [SerializeField] Transform gateChild; // Stores the Transform component of the moving part of the gate
    private BoxCollider2D collider; // Stores the gateChild's collider
    [SerializeField] bool defaultStateOpen; // Determines if the gate should be open at the start of the level or not.
    [SerializeField] bool isOpen; // Is the gate open or closed?

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        gateChild = transform.Find("Gate"); // Searches for the Transform component of a gameObject named "Gate"
        collider = gateChild.GetComponent<BoxCollider2D>(); // Assigns the gateChild's collider to the variable
        animator = GetComponent<Animator>();
        isOpen = defaultStateOpen; // Whatever the gate's default state is, set it.
    }
    void Update()
    {
        // If the game JUST entered gamestate 1 (the state before a level begins) the frame before...
        if (GameManager.gameState == 1 && GameManager.framesSinceStateChange == 1)
        {
            // Set the gate to the wrong position, then use MoveGate() to set it to the starting position
            isOpen = !defaultStateOpen;
            MoveGate();
        }

        // If the gate is opened, set the collider to be a trigger. This prevents the player from getting stuck on protrusions.
        if (isOpen) { collider.isTrigger = true; }
        else { collider.isTrigger = false; }
    }
    // If the gate is open, close it, if it is closed, then open it.
    // This function is meant to be called by an external event, such as a pushed button.
    public void MoveGate()
    {
        audio.Play();
        if (isOpen) { animator.SetTrigger("CloseGate"); }
        else { animator.SetTrigger("OpenGate"); }
        isOpen = !isOpen;
    }
}
