using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        if (MatchManager.Instance == null) return;
        Debug.Log("lololo " + teamAlignment);
        sr.sprite = numberSprites[Math.Clamp(0, MatchManager.Instance.lives[teamAlignment], 9)];
    }



}
