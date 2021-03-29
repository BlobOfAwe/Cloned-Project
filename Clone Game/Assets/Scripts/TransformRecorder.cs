using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
    public List<Vector2> playerTransforms; // A list that holds all of the player's positions for that attempt
    public List<Vector2> playerScales; // A list that holds all changes to a player's scale for that attempt
    [SerializeField] GameObject clonePrefab; // Holds the prefab to create a clone after every attempt

    private void Update()
    {
        Clone();
    }
    
    void FixedUpdate()
    {
        if (GameManager.gameState == 2)
        {
            playerTransforms.Add(transform.position);
            playerScales.Add(transform.localScale);
        }
    }

    // Check to see if an attempt has JUST finished, then create a clone, and attatch the playerTransforms and playerScales from that attempt.
    void Clone()
    {
        // IF the gamemanager entered state 1 (the state before a level starts) one frame ago, and the playerTransforms have been recorded...
        if(GameManager.gameState == 1 && GameManager.framesSinceStateChange == 1 && playerTransforms.Count > 0)
        {
            GameObject newClone = Instantiate(clonePrefab, new Vector3(transform.position.x,transform.position.y), Quaternion.identity); // Instantiate the clone prefab
            newClone.GetComponent<CloneController>().transforms = new List<Vector2>(playerTransforms); // Attatch the previous attempt's transforms to the newClone
            newClone.GetComponent<CloneController>().scales = new List<Vector2>(playerScales); // Attatch the previous attempt's scales to the newClone
            playerTransforms.Clear(); // Clear the player's transform and scale list, to allow for recording of the next attempt.
            playerScales.Clear();
        }
    }
}
