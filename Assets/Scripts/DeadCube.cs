using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCube : MonoBehaviour
{
    public int teamAlignment = -1;

    public NumberDisp scoreDisp;

    // true when this cube has already decremented score
    private bool accounted = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (teamAlignment == -1 || accounted) return;

        MatchManager.Instance.lives[teamAlignment] -= 1;
        accounted = true;
        scoreDisp.Refresh();

        // this player has lost
        if (MatchManager.Instance.lives[teamAlignment] <= 0)
        {
            MatchManager.Instance.teamPlatforms[teamAlignment].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
