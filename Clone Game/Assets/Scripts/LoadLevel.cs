using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel: MonoBehaviour
{
    public void LoadScene(int level)
    {
        SceneManager.LoadSceneAsync(level);
        
    }
}
