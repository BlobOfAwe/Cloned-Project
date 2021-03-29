using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the clones based on a list of transforms and scales from TransformRecorder. These lists match the positions and scales of the player during the last attempt.
/// </summary>
public class CloneController : MonoBehaviour
{
    public GameManager gameManager;
    public List<Vector2> transforms;
    public List<Vector2> scales;
    
    private int listIndex; // Identifies which index each list should read. 

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Identify the gameManager
        listIndex = 1; // Set the list Index to 1
    }
    private void Update()
    {
        // Each time the level resets, restart the script
        if (GameManager.gameState == 1)
        {
            Start();
        }
    }
    
    // Each fixedUpdate...
    private void FixedUpdate()
    {
        // If the level is in progress and listIndex has not exceeded the number of items in each list.
        if (GameManager.gameState == 2 && listIndex < transforms.Count)
        {
            transform.position = transforms[listIndex]; // Set the clone's position to [listIndex] of transforms
            transform.localScale = scales[listIndex]; // Set the clone's scale to [listIndex] of scales
            listIndex += 1; // Increase ListIndex by 1
        }

    }
}
