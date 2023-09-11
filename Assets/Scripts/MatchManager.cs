using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

    public static MatchManager Instance;

    [SerializeField] private int startingLifeCount = 5;
    public int[] lives;

    [SerializeField] NumberDisp[] scoreDisps;

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
