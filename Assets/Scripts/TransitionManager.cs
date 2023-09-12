using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    // void Start()
    // {
    //     DontDestroyOnLoad(gameObject);
    // }
    public void SetScene(string s)
    {
        SceneManager.LoadScene(s);
    }
}
