using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SummonDisp : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    public void Refresh(float fillAmt)
    {
        if (fillAmt <= 0.15f) fillAmt = 0f;
        transform.localScale = new Vector2(transform.localScale.x, fillAmt);
    }
}
