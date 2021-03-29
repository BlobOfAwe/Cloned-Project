using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the timer in each level.
/// </summary>
public class Timer : MonoBehaviour
{
    public int timer = 5; // The time limit of each level
    public float timeRemaining; // The amount of time left in each level.
    public bool isTiming; // Is the timer running currently?
    private TextMeshProUGUI timerTextMesh; // The text displaying the timer
    private GameManager gameManager;

    [Header("Unused")]
    // Originally purposed for a visual timer.
    [SerializeField] GameObject clockIcon;
    [SerializeField] Sprite clockFullSpr;
    [SerializeField] Sprite clockEmptySpr;
    private Image clockSprite;


    // Start is called before the first frame update
    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Identify the gameManager
        isTiming = false; // Do not start the timer
        timerTextMesh = GetComponent<TextMeshProUGUI>(); // Identify the timer text
        timeRemaining = timer; // Set the time remaining to the amount of time for the level
    }

    private void Update()
    {
        if (GameManager.gameState != 2) { isTiming = false; } // If the gameState is anything but 2 (Level in progress), stop the timer
        else { isTiming = true; } // If the level is in progress, run the timer
    }

    // Runs after Update
    private void LateUpdate()
    {
        // If there is time remaining, and the timer is running...
        if(timeRemaining > 0 && isTiming)
        {
            timeRemaining -= Time.deltaTime; // Subtract the time since the last LateUpdate call from timeRemaining
            timerTextMesh.text = ("Timer: " + Convert.ToString(Math.Ceiling(timeRemaining))); // Set the timer text to the time remaining, rounded up to the nearest whole number
        }
    }


}
