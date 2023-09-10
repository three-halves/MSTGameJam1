using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDisp : MonoBehaviour
{
    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private int teamAlignment;
    [SerializeField] private SpriteRenderer sr;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (StorageAsset.Instance == null) return;
        Debug.Log("lololo");
        sr.sprite = numberSprites[StorageAsset.Instance.lives[teamAlignment]];
    }



}
