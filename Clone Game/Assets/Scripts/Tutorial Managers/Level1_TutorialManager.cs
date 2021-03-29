using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Level1_TutorialManager : MonoBehaviour
{
    // sign_1 in this level tells the player how to start the level
    [SerializeField] GameObject sign_1;

    // sign_2 in this level tells the player how to move
    [SerializeField] GameObject sign_2;

    // Start is called before the first frame update
    void Start()
    {
        sign_1.SetActive(true);
        sign_2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Once the player has pressed enter, they know how to start the level. Remove that sign and replace it with the next one
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sign_1.SetActive(false);
            sign_2.SetActive(true);
        }
    }
}
