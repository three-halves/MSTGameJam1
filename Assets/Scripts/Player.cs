using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // controls for this player. different for p1 and p2
    [SerializeField] private KeyCode KJump;
    [SerializeField] private KeyCode KLeft;
    [SerializeField] private KeyCode KRight;
    [SerializeField] private KeyCode KDown;
    [SerializeField] private KeyCode KThrow;
    [SerializeField] private KeyCode KAltThrow;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpControl;
    [SerializeField] private float walkAccel;
    [SerializeField] private float fric;
    [SerializeField] private float pSpeed;
    [SerializeField] private float jumpBufferTime;

    // 0 for red, 1 for blue
    [SerializeField] public int teamAlignment;
    [SerializeField] public string noCollideTag;

    [SerializeField] public float cubeSummonTime;
    // multiplier for how long the next summon will take. reset each summon
    private float nextSummonMultiplier = 1f;

    // -1 when left, 1 when right. never 0 or any "neutral" value
    private int facing = 1;

    private int axisX;

    // used for cube holding logic
    [SerializeField] private GameObject cubePrefab;
    public GameObject holding;
    
    [SerializeField] private SummonDisp summonDisp;

    // used to enforce max cubes per player.
    [SerializeField] public List<GameObject> ownedCubes = new List<GameObject>();

    // used as temp variable to modify rb velocity
    private Vector2 vel;

    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    // inputs gathered during Update() to be read in FixedUpdate()
    private bool jumpPressed;
    private bool jumpHeld;
    // time since jump was last input
    private float jumpTimer;
    private float throwTimer;
    private float summonTimer;
    private bool downPressed;
    private bool throwPressed;

    // unity layer for solid ground
    [SerializeField] public LayerMask groundLayer;
    // unity layer for one-way platforms
    // [SerializeField] public LayerMask platformLayer;

    [SerializeField] SpriteRenderer sr;
    // animation frames IN ORDER stand hold throw
    [SerializeField] Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        rb.gravityScale = 3;

        sr.sprite = sprites[0];
    }

    // Update is called once per frame
    // gather player input
    void Update()
    {

        axisX = (Input.GetKey(KRight) ? 1 : 0) - (Input.GetKey(KLeft) ? 1 : 0);
        if (axisX != 0) facing = axisX;

        // jumping
        if (Input.GetKeyDown(KJump))
        {
            jumpPressed = true;
            jumpTimer = 0f;
        }

        if (Input.GetKeyDown(KThrow) || Input.GetKeyDown(KAltThrow)) throwTimer = 0f;

        jumpTimer += Time.deltaTime;
        throwTimer += Time.deltaTime;

        if (holding == null && (Input.GetKey(KThrow) || Input.GetKey(KAltThrow))) summonTimer += Time.deltaTime;
        else summonTimer = 0f;

        downPressed = Input.GetKey(KDown);
        throwPressed = throwTimer <= 0.1f;
        jumpHeld = Input.GetKey(KJump);

        // update position of held obj
        if (holding) holding.transform.position = new Vector2(transform.position.x, transform.position.y + 1.5f);
    }

    // fixed update is called at a fixed interval given by simulation steps
    // process player input
    void FixedUpdate()
    {
        // if(teamAlignment == 0) Debug.Log(throwTimer);
        // assign rigidbody velocity to new variable for easier modifying
        vel = rb.velocity;

        // physics calculation to vel

        // jumping   
        if (jumpTimer <= jumpBufferTime && jumpPressed && Grounded())
        {
            vel.y = jumpHeight;
            jumpPressed = false;
        }

        // jump control
        if (!jumpHeld && vel.y > 0)
        {
            vel.y *= jumpControl;
        }

        // walking
        // apply a bit extra accel when turning around on ground
        vel.x += (float) (walkAccel * axisX);
        if (axisX == 0) vel.x *= fric;

        // cap x speed
        if (Math.Abs(vel.x) > pSpeed) vel.x = pSpeed * (Math.Abs(vel.x)/vel.x);

        // mininum horizontal speed to avoid tiny floats
        if (Math.Abs(vel.x) < 0.01) vel.x = 0;



        // throw object
        if (throwPressed && holding)
        {
            holding.GetComponent<Cube>().source.PlayOneShot(holding.GetComponent<Cube>().throwSFX);
            Throw(holding, Math.Min(Math.Abs(rb.velocity.x * 1.4f) * (teamAlignment * 2 - 1) * -1, 20f), Math.Min(rb.velocity.y * 2f + 5f, 15f));
        }
        if (downPressed && holding)
        {
            holding.GetComponent<Cube>().source.PlayOneShot(holding.GetComponent<Cube>().spikeSFX);
            Throw(holding,  13f * (teamAlignment * 2 - 1) * -1, -10f, 0.2f);
            if (!Grounded()) vel.y = jumpHeight * 1f;
            nextSummonMultiplier = 1.5f;
           
            
        }

        // summon cube if button is held long enough
        summonDisp.Refresh(summonTimer / (cubeSummonTime * nextSummonMultiplier));
        if (summonTimer >= cubeSummonTime * nextSummonMultiplier)
        {
            GameObject newCube = Instantiate(cubePrefab);
            newCube.GetComponent<Cube>().teamAlignment = teamAlignment;
            Grab(newCube);
            holding.GetComponent<Cube>().source.PlayOneShot(holding.GetComponent<Cube>().summonSFX);
            nextSummonMultiplier = 1f;
        }

        // apply vel to actual velocity of rigidbody
        rb.velocity = new Vector2(vel.x, vel.y);

    } // close FixedUpdate()

    // called from a cube when collided with
    public void Grab(GameObject other)
    {
        // early return if already grabbing something
        if (holding != null) return;

        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();

        holding = other;
        otherRb.isKinematic = true;
        otherRb.velocity = Vector2.zero;
        // negate velocity change from collision
        rb.velocity = vel;
        // Debug.Log("Grabbed " + other);
        sr.sprite = sprites[1];
        holding.GetComponent<Cube>().source.PlayOneShot(holding.GetComponent<Cube>().grabSFX);
    }

    public void Throw(GameObject other, float xspd, float yspd, float collisionTimer = 0.1f)
    {
        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();

        sr.sprite = sprites[2];
        StartCoroutine(setSpriteWithDelay(0,0.25f));

        other.tag = noCollideTag;
        other.gameObject.GetComponent<Cube>().RefreshCollision(this);
        otherRb.isKinematic = false;
        otherRb.velocity = new Vector2(xspd, yspd);
        holding = null;
        StartCoroutine(resetTag(other, collisionTimer));

    }

    public void AddToOwned(GameObject o)
    {
        ownedCubes.Add(o);
        if (ownedCubes.Count > MatchManager.Instance.maxCubes)
        {
            Destroy(ownedCubes[0]);
            ownedCubes.RemoveAt(0);
        }

    }

    private IEnumerator setSpriteWithDelay(int i, float t)
    {
        yield return new WaitForSeconds(t);
        sr.sprite = sprites[i];
    }

    private IEnumerator resetTag(GameObject other, float collisionTimer)
    {
        yield return new WaitForSeconds(collisionTimer);
        other.tag = "Untagged";
        other.gameObject.GetComponent<Cube>().RefreshCollision(this);
    }

    public bool Grounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        if ((hit == null) || (hit.collider == null)) return false;
        if (hit.collider.gameObject.GetComponent<Cube>() == null) return true;
        return (hit.collider.gameObject.GetComponent<Cube>().teamAlignment == teamAlignment);
    }
}
