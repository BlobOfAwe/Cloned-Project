using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages data specific to each level, particularily data about frames, time, and clones
/// </summary>
public class LevelManager : MonoBehaviour
{
    public int cloneLimit = 2; // Amount of clones a player can create per level. Default is two, max is 4.
    public int clonesRemaining; // Amount of clones the player can still create in each level
    private Array clones; // An array of clones in a level
    [SerializeField] List<GameObject> cloneIcons; // A list of the icons representing clonesRemaining

    void Awake()
    {
        // Assigns the four Clone_Icon gameObjects to the list
        for (int i = 0; i <= 3; i++)
        {
            cloneIcons[i] = GameObject.Find("Clone_Icon (" + Convert.ToString(i+1) + ")");
        }

        CloneListUpdate();

    }
    
    // Update is called once per frame
    void Update()
    {
        // If the gamestate has JUST entered state 1 (the state before a level begins)...
        if (GameManager.gameState == 1 && GameManager.framesSinceStateChange == 2) // framesSinceStateChange should be the frame AFTER the clones are instantiated, which is frame 1
        {
            // Assign all existing clones in a level to an array
            clones = GameObject.FindGameObjectsWithTag("Clone");

            // If more clones are found than are allowed...
            if (clones.Length > cloneLimit)
            {
                // Destroy all clones
                foreach (GameObject x in clones) { Destroy(x); }
            }
        }

        // ONCE, when the gamestate changes to 1, the game updates the cloneList
        if(GameManager.gameState == 1 && GameManager.framesSinceStateChange == 3) // framesSinceStateChange should be the frame after all clones have been instantiated, and after the destroy-check above
        {
            CloneListUpdate();            
        }
    }

    // Activates and Deactivates clone icons based on how many clones the player has left to use
    void CloneListUpdate()
    {
        clones = GameObject.FindGameObjectsWithTag("Clone");
        clonesRemaining = cloneLimit - clones.Length;
        
        // Four times (the max value of cloneLimit)
        for (int i = 0; i < 4; i++)
        {
            // If i is lower than the number of clones remaining...
            if (i < clonesRemaining)
            {
                // Display this cloneIcon.
                cloneIcons[i].SetActive(true);
            }

            else
            {
                cloneIcons[i].SetActive(false);
            }

        }
    }
}
