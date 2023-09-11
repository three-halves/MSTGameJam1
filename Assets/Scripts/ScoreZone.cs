using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    // 0 red, 1 blue
    [SerializeField] public int teamAlignment;

    // the number disp object this score zone effects
    [SerializeField] NumberDisp scoreDisp;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null) StartCoroutine(HandlePlayerTrigger(other));
        if (other.gameObject.GetComponent<Cube>() != null) HandleCubeTrigger(other);
    }

    private IEnumerator HandlePlayerTrigger(Collider2D other)
    {
        yield return new WaitForSeconds(1f);
        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
        otherRb.velocity = Vector2.zero;
        other.gameObject.transform.position = new Vector2(4f * (teamAlignment * 2 - 1), 6f);
    }

    private void HandleCubeTrigger(Collider2D other)
    {
        // Debug.Log("Cube score");
        if (teamAlignment != other.GetComponent<Cube>().teamAlignment)
        {
            MatchManager.Instance.lives[teamAlignment] -= 1;
        }
        scoreDisp.Refresh();
    }
}
