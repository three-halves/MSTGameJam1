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

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpControl;
    [SerializeField] private float walkAccel;
    [SerializeField] private float fric;
    [SerializeField] private float pSpeed;
    [SerializeField] private float jumpBufferTime;

    // -1 when left, 1 when right. never 0 or any "neutral" value
    [SerializeField] private int facing = 1;

    private int axisX;

    // used as temp variable to modify rb velocity
    private Vector2 vel;

    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    // inputs gathered during Update() to be read in FixedUpdate()
    private bool jumpPressed;
    private bool jumpUnpressed;
    // time since jump was last input
    private float jumpTimer;

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
        // Debug.Log("erm");
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
    }

    // fixed update is called at a fixed interval given by simulation steps
    // process player input
    void FixedUpdate()
    { 
        Debug.Log(axisX);
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

        // soft cap horziontal speed
        if (Math.Abs(vel.x) > pSpeed) vel.x -= walkAccel * (Math.Abs(vel.x)/vel.x);

        // mininum horizontal speed to avoid tiny floats
        if (Math.Abs(vel.x) < 0.01) vel.x = 0;


        // apply vel to actual velocity of rigidbody
        rb.velocity = new Vector2(vel.x, vel.y);

    } // close FixedUpdate()

    public bool Grounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }
}


