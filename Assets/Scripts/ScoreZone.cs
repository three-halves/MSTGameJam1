using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class ScoreZone : MonoBehaviour
{
    // 0 red, 1 blue
    [SerializeField] public int teamAlignment;

    // the number disp object this score zone effects
    [SerializeField] NumberDisp scoreDisp;
    
    // dropped on score
    [SerializeField] GameObject deadCubePrefab;
    
    // used to start postgame
    [SerializeField] PostGameWindow postGameWindow;
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

        // respawn player
        if (MatchManager.Instance.lives[teamAlignment] > 0)
        {
            Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
            otherRb.velocity = Vector2.zero;
            other.gameObject.transform.position = new Vector2(4f * (teamAlignment * 2 - 1), 6f);
        }
        // end game
        else
        {
            yield return new WaitForSeconds(1f);
            postGameWindow.postGameRoutine(1 - teamAlignment);
        }

    }

    private void HandleCubeTrigger(Collider2D other)
    {
        // Debug.Log("Cube score");
        if (teamAlignment != other.GetComponent<Cube>().teamAlignment)
        {
            // spawn dead cube
            GameObject newDC = Instantiate(deadCubePrefab);
            newDC.transform.position = new Vector2(Random.Range(0.5f,6f) * (teamAlignment * 2 - 1), 6.5f);
            newDC.GetComponent<DeadCube>().teamAlignment = teamAlignment;
            newDC.GetComponent<DeadCube>().scoreDisp = scoreDisp;

            StartCoroutine(other.GetComponent<Cube>().DestroyWithDelay(0.1f));
        }
        // scoreDisp.Refresh();
    }
}
