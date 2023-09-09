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

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpControl;
    [SerializeField] private float walkAccel;
    [SerializeField] private float fric;
    [SerializeField] private float pSpeed;
    [SerializeField] private float jumpBufferTime;

    // 0 for red, 1 for blue
    [SerializeField] public int teamAlignment;

    // -1 when left, 1 when right. never 0 or any "neutral" value
    private int facing = 1;

    private int axisX;

    // used for cube holding logic
    private GameObject holding;

    // used as temp variable to modify rb velocity
    private Vector2 vel;

    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    // inputs gathered during Update() to be read in FixedUpdate()
    private bool jumpPressed;
    private bool jumpUnpressed;
    // time since jump was last input
    private float jumpTimer;
    private bool downPressed;
    private bool throwPressed;

    // unity layer for solid ground
    [SerializeField] public LayerMask groundLayer;
    // unity layer for one-way platforms
    // [SerializeField] public LayerMask platformLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        rb.gravityScale = 3;
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
            jumpTimer = 0;
        }

        if (Input.GetKeyUp(KJump))
        {
            jumpUnpressed = true;
        }

        jumpTimer += Time.deltaTime;

        downPressed = Input.GetKey(KDown);
        throwPressed = Input.GetKey(KThrow);

        // update position of held obj
        if (holding) holding.transform.position = new Vector2(transform.position.x, transform.position.y + 1.5f);
    }

    // fixed update is called at a fixed interval given by simulation steps
    // process player input
    void FixedUpdate()
    {
        // Debug.Log(Time.deltaTime);
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
        if (jumpUnpressed)
        {
            jumpUnpressed = false;
            if (vel.y > 0) vel.y *= jumpControl;
        }

        // walking
        // apply a bit extra accel when turning around on ground
        vel.x += (float) (walkAccel * axisX);
        if (axisX == 0) vel.x *= fric;

        // cap x speed
        if (Math.Abs(vel.x) > pSpeed) vel.x = pSpeed * (Math.Abs(vel.x)/vel.x);

        // mininum horizontal speed to avoid tiny floats
        if (Math.Abs(vel.x) < 0.01) vel.x = 0;

        // apply vel to actual velocity of rigidbody
        rb.velocity = new Vector2(vel.x, vel.y);


        // throw object
        if (throwPressed && holding) Throw(holding);

    } // close FixedUpdate()

    // called from a cube when collided with
    public void Grab(GameObject other)
    {
        // early return if already grabbing something
        if (holding != null) return;

        holding = other;
        other.GetComponent<Rigidbody2D>().isKinematic = true;
        Debug.Log("Grabbed " + other);
    }

    public void Throw(GameObject other)
    {
        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
        otherRb.isKinematic = false;
        otherRb.velocity = new Vector2(rb.velocity.x * 2f, rb.velocity.y * 2f + 5f);
        holding = null;

    }

    public bool Grounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }
}


