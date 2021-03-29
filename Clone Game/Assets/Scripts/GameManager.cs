using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages Full-game data. Including gamestates and frames.
/// </summary>
public class GameManager : MonoBehaviour
{
    // General variables
    private GameObject player;
    private Array cloneObjects;
    public static int sceneIndex;


    // gameState 0 == Main Menu
    // gameState 1 == Pre-Start
    // gameState 2 == Level in progress
    public static int gameState;
    [SerializeField] int gameStateIndicator; // This variable should only be used to make gameState visible in the inspector

    // The number of frames (called in Update()) since the last gameState change
    public static int framesSinceStateChange;
    [SerializeField] int framesSinceStateChangeIndicator; // This variable should only be used to make framesSince... visible in the inspector

    [Header("Debug Variables")]
    public bool debug_on;
    [SerializeField] float timeRate;

    
    // As soon as this gameObject is rendered...
    private void Awake()
    {
        gameState = 1; // Set the game-state to pre-level
        player = GameObject.Find("Player"); // Identify the player
        framesSinceStateChange = 0;
    }

    // On the first frame...
    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex; // Get this scene's Index from the SceneManager
    }
    
    // Each frame...
    private void Update()
    {
        // Update the Indicators in the inspector. FOR DEV PURPOSES ONLY, THESE ARE NOT USED BY THE GAME
        gameStateIndicator = gameState;
        framesSinceStateChangeIndicator = framesSinceStateChange;

        // FOR DEV PURPOSES ONLY
        // Debug mode allows time to be slowed down or sped up.
        if (debug_on) { Time.timeScale = timeRate; }
        else { Time.timeScale = 1; }


        // If the level has not started (gamestate 1)...
        if (gameState == 1)
        {
            // If the player hits the ENTER key...
            if (Input.GetKeyDown(KeyCode.Return)) 
            {
                ChangeGameState(2); // Start the level
                player.GetComponent<PlayerController>().isControllable = true ; // Make the player controllable
            }

            // If the player hits the R key...
            else if (Input.GetKeyDown(KeyCode.R))
            {
                cloneObjects = GameObject.FindGameObjectsWithTag("Clone"); // Identify all clones

                foreach(GameObject x in cloneObjects) { Destroy(x); } // Destroy all clones
            }

            // On frame 0 of gamestate 1, disable player control
            else if(framesSinceStateChange <= 1)
            { 
                player.GetComponent<PlayerController>().isControllable = false; 
            }
        }

        framesSinceStateChange++; // Update framesSince...
    }

    // Changes the current gamestate to the specified integer
    // gameState 0 == Main Menu
    // gameState 1 == Pre-Start
    // gameState 2 == Level in progress
    public void ChangeGameState(int state)
    {
        framesSinceStateChange = 0; // reset framesSince...
        gameState = state;
    }

}