using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

    public static MatchManager Instance;

    private int startingLifeCount;
    public int[] lives;

    public int maxCubes;

    [SerializeField] NumberDisp[] scoreDisps;
    [SerializeField] public Player[] players;

    // index based on team alignment, used when one side loses
    [SerializeField] public GameObject[] teamPlatforms;

    // match set up
    void Start()
    {
        Instance = this;
        startingLifeCount = PlayerPrefs.GetInt("StartingLives", 5);
        maxCubes = PlayerPrefs.GetInt("MaxCubes", 4);
        lives = new int[]{startingLifeCount,startingLifeCount};

        foreach(NumberDisp n in scoreDisps)
        {
            n.Refresh();
        }

        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
