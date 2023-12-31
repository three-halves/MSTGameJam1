using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // 0 red, 1 blue, -1 neutral
    [SerializeField] public int teamAlignment = -1;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite[] teamSprites;

    [SerializeField] private float bounceHeight;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] public AudioSource source;
    [SerializeField] public AudioClip spikeSFX;
    [SerializeField] public AudioClip grabSFX;
    [SerializeField] public AudioClip throwSFX;
    [SerializeField] public AudioClip summonSFX;
    [SerializeField] public AudioClip jumpedSFX;
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
            source.PlayOneShot(jumpedSFX);
    
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
        if (teamAlignment != -1 && MatchManager.Instance.players[teamAlignment].ownedCubes.Contains(gameObject)) MatchManager.Instance.players[teamAlignment].ownedCubes.Remove(gameObject);
        teamAlignment = team;
        sr.sprite = teamSprites[teamAlignment+1];
        if (team != -1) MatchManager.Instance.players[teamAlignment].AddToOwned(gameObject);
    }

    public void RefreshCollision(Player other)
    {
        Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), gameObject.tag.Equals(other.noCollideTag));
        // Debug.Log("Collide check " + gameObject.tag.Equals(other.noCollideTag));
    }

    public IEnumerator DestroyWithDelay(float t)
    {
        yield return new WaitForSeconds(t);
        MatchManager.Instance.players[teamAlignment].ownedCubes.Remove(gameObject);
        // Debug.Log("Sad :)");
        Destroy(gameObject);
    }
}
