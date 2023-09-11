using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

    public static MatchManager Instance;

    [SerializeField] private int startingLifeCount = 5;
    public int[] lives;

    [SerializeField] NumberDisp[] scoreDisps;

    // index based on team alignment, used when one side loses
    [SerializeField] public GameObject[] teamPlatforms;

    // match set up
    void Start()
    {
        Instance = this;
        lives = new int[]{startingLifeCount,startingLifeCount};

        foreach(NumberDisp n in scoreDisps)
        {
            n.Refresh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
