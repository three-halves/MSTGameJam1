using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    // match set up
    void Start()
    {
        StorageAsset.Instance.lives = new int[]{5,5};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
