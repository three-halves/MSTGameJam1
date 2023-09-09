using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // 0 red, 1 blue, -1 neutral
    [SerializeField] int teamAlignment = -1;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] private Color[] teamColors;

    // Start is called before the first frame update
    void Start()
    {
        UpdateColor();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null) return; 

        Debug.Log("player cube collide");
        Player player = collision.gameObject.GetComponent<Player>();

        // claim cube if unaligned
        if (teamAlignment == -1)
        {
            teamAlignment = player.teamAlignment;
            UpdateColor();
        }

        // pick up cube
        if (teamAlignment == player.teamAlignment)
        {
            player.Grab(gameObject);
        }
    }

    void UpdateColor()
    {
        sr.color = teamColors[teamAlignment+1];
    }
}
