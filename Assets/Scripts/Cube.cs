using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // 0 red, 1 blue, -1 neutral
    [SerializeField] public int teamAlignment = -1;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color[] teamColors;

    [SerializeField] private float bounceHeight;

    [SerializeField] private Rigidbody2D rb;
    // player who last collided with this cube
    private Player collidingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTeam(teamAlignment);
        // rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<Player>() == null) return; 

        // Debug.Log("player cube collide");
        collidingPlayer = collision.gameObject.GetComponent<Player>();

        // claim cube if unaligned
        if (teamAlignment == -1)
        {
            UpdateTeam(collidingPlayer.teamAlignment);
        }

        // pick up cube if correct alignment
        if (teamAlignment == collidingPlayer.teamAlignment)
        {
            collidingPlayer.Grab(gameObject);
        }
        // do enemy collision
        else HandlePlayerCollision(collision);
    }

    void HandlePlayerCollision(Collision2D collision)
    {
        Bounds cubeBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds playerBounds = collision.collider.bounds;

        // height difference between bottom of player and top of cube
        float heightDiff = (cubeBounds.center.y + cubeBounds.extents.y) - (playerBounds.center.y - playerBounds.extents.y);

        // jumping on cube
        if ((collision.rigidbody.velocity.y <= 0) && (heightDiff <= 0.2f))
        {
            rb.velocity = Vector2.zero;
            UpdateTeam(collidingPlayer.teamAlignment);
            collision.rigidbody.velocity = collision.rigidbody.velocity + Vector2.up * bounceHeight;

        }

        // grabbing from bottom
        // Debug.Log(rb != null);
        if ((rb.velocity.y < 0) && (heightDiff >= 0.2f) && collidingPlayer.holding == null)
        {
            UpdateTeam(collidingPlayer.teamAlignment);
            collidingPlayer.Grab(gameObject);
        }
    }

    public void UpdateTeam(int team)
    {
        teamAlignment = team;
        sr.color = teamColors[teamAlignment+1];
    }

    public void RefreshCollision(Player other)
    {
        Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), gameObject.tag.Equals(other.noCollideTag));
        Debug.Log("Collide check " + gameObject.tag.Equals(other.noCollideTag));
    }
}
