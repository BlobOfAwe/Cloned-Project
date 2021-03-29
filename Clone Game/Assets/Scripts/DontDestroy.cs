using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Used by the Music Player to prevent the music from stopping inbetween scenes.
/// </summary>
public class DontDestroy : MonoBehaviour
{
    [SerializeField] string id_tag; // Stores the tag used to identify the music player
    
    // As soon as this gameObject loads...
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(id_tag); // Check for all gameObjects with the specified tag
        
        if(objs.Length > 1) { Destroy(this.gameObject); } // If there is more than one gameObject with this tag, the object running this script is redundant and should be destroyed
        
        DontDestroyOnLoad(this.gameObject); // If this is the only gameObject with this tag, prevent it from being destroyed inbetween scenes.
    }
}
