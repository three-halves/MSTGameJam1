using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMSource : MonoBehaviour
{
    public static BGMSource Instance;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip battleMusic;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        Instance.SetMusic();

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Strange");
        SetMusic();
    }

    public void SetMusic()
    {
        Debug.Log("Music block running");
        AudioClip toSet;
        if (SceneManager.GetActiveScene().name.Equals("Main")) toSet = battleMusic;
        else toSet = menuMusic;

        Debug.Log(SceneManager.GetActiveScene().name);

        if (source.clip != toSet)
        {
            source.clip = toSet;
            source.Play();
        }
    }

}
