using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player's health, death, and respawn.
/// </summary>
public class HealthManager : MonoBehaviour
{
    private PlayerController playerController; // The script that controls the player
    private GameManager gameManager;
    private Rigidbody2D rb; // The player's rigidbody
    private AudioPlayer audioPlayer;
    [SerializeField] PhysicsMaterial2D bounceMaterial; // A bouncy material
    [SerializeField] GameObject respawn; // The respawn point
    [SerializeField] int BaseHP = 1; // The amount of health the player starts the level with. By default, this is 1
    [SerializeField] int hp; // The player's current health
    [SerializeField] float knockback = 5f; // The force with which the player is knocked backwards after a hit
    [SerializeField] float knockup = 5f; // the force with which the player is knocked upwards after a hit
    [SerializeField] float stunTime = 2f; // the time the player is stunned after a hit

    [SerializeField] GameObject timerObject; // The in-game timer
    private Timer timer; // The script that runs the timer
    public bool dead = false; // Is the player dead?

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timer = timerObject.GetComponent<Timer>();
        rb = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponent<AudioPlayer>();
        hp = BaseHP;
        transform.position = respawn.transform.position; // set the player to the spawn position
    }

    private void Update()
    {
        // If the player runs out of time, or ends the attempt early, kill them.
        if (Input.GetKey(KeyCode.DownArrow) || timer.timeRemaining <= 0)
        {
            if(GameManager.gameState == 2)
            {
                hp = 0;
                Respawn();
            }
        }
    }
    
    // Ends the player's attempt and returns them to the respawn point
    public void Respawn()
    {
        playerController.isControllable = false; // Prevent the player from moving
        dead = true; // Tell the program the player has died
        gameManager.ChangeGameState(1); // GameState 1 before the level begins
        rb.velocity = Vector2.zero; // Set the player's velocity to 0 (this is to prevent momentum from carrying over between attempts)
        transform.position = respawn.transform.position; // Set the player to the respawn position
        timer.Start(); // Reset the timer
        hp = BaseHP; // Reset the player's health
    }

    IEnumerator Stun()
    {
        playerController.isControllable = false; // Prevent the player from moving while stunned
        rb.sharedMaterial = bounceMaterial; // Makes the player bouncy while stunned
        rb.velocity = (-transform.right * knockback) + (transform.up * knockup); // Knocks the player back based on knockback and knockup
        yield return new WaitForSecondsRealtime(stunTime); // maintain the stunned state for stunTime seconds
        if(GameManager.gameState == 2) { playerController.isControllable = true; } // If the level is currently in progress (state 2) then make the player controllable again
        rb.sharedMaterial = null; // Reset the player's material so they are no longer bouncy
    }

    // IF the player hits an enemy, stun them and reduce HP. If HP is 0, then kill them
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check to see if the gameObject the player collided with was an enemy
        if (collision.CompareTag("Enemy")){
            audioPlayer.PlayAudio(); // Play the hurt-sound
            hp -= 1; // Reduce hp by 1
            if(hp <= 0) { Respawn(); } // If hp is 0, end the attempt and respawn
            // If hp is greater than 0, stun the player.
            else if(hp > 0)
            {
                StartCoroutine(Stun());   
            }
        }
    }
}
