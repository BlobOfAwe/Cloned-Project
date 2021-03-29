using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the Tutorial signs in Level 4
public class Level4_TutorialManager : MonoBehaviour
{
    private LevelManager levelManager;

    [SerializeField] GameObject sign_1; // "This ledge is too high to jump over, keep trying anyway!"
    [SerializeField] GameObject sign_2; // "Now you have a clone! use it to jump over!"

    private void Start()
    {
        levelManager = gameObject.GetComponent<LevelManager>();
    }
    // Update is called once per frame
    void Update()
    {
        // If the clone has not been created (ie there are still clones left), display sign 1
        if (levelManager.clonesRemaining == 1)
        {
            sign_1.SetActive(true);
            sign_2.SetActive(false);
        }

        // If there is a clone in the scene (ie. there are no clones left), display sign 2
        else if (levelManager.clonesRemaining == 0)
        {
            sign_1.SetActive(false);
            sign_2.SetActive(true);
        }

        // If the program detects an amount of clones remaining other than 0 and 1, return an error
        else
        {
            Debug.LogError("Invalid value for LevelManager.clonesRemaining (Error from Level4_TutorialManager)");
        }
    }
}
