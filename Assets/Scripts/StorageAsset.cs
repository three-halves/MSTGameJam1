using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton class used to store / serialize data that can be used at any time
[CreateAssetMenu(fileName = "Storage Asset", menuName = "ACMGameJam/StorageAsset")]
public class StorageAsset : ScriptableObject
{
    public static StorageAsset Instance;

    // set instance so asset can be statically referenced
    void OnEnable()
    {
        Debug.Log("Storage Instance Enabled");
        Instance = this;
    }

    // lives of red and blue player during match (index 0 and 1)
    [SerializeField] public int startingLifeCount = 5;
    public int[] lives = new int[] {5,5};

}