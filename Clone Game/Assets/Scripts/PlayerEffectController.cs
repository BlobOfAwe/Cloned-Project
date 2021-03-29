using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls effects used by the player gameObject
/// </summary>
public class PlayerEffectController : MonoBehaviour
{
    private GameObject jumpEffect; // gameObject used for the Jump effect
    private Animator jumpAnimator; // Animator attatched to the jumpEffect GameObject
    private float baseOfPlayer = 0.5f; // Identifies the point for the jump effect to appear, relative to the player

    // Start is called before the first frame update
    void Start()
    {
        jumpEffect = GameObject.Find("Jump_Effect"); // Identify the JumpEffect gameObject
        jumpAnimator = jumpEffect.GetComponent<Animator>(); // Identify the JumpEffect's animator
    }

    public IEnumerator PlayJump()
    {
        jumpEffect.transform.position = new Vector2(transform.position.x, transform.position.y - baseOfPlayer); // Set the jump effect object to the baseOfPlayer
        jumpAnimator.SetTrigger("play_trig"); // Set the trigger to play the animation
        yield return new WaitForEndOfFrame();
        jumpAnimator.ResetTrigger("play_trig"); // As soon as the frame ends, deactivate the trigger (this is to prevent stuttering in the animation)
    }
}
