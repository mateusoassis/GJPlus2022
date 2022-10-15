using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManagerScript;
    [SerializeField] private CollisionChecking colScript;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode wallGrabKey;
    [SerializeField] private KeyCode dashKey;

    [Header("Inputs")]
    private float xInput;
    private float xRawInput;
    private float yInput;
    private float yRawInput;
    [SerializeField] private Vector2 walkDirection;
    
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float walkSpeed;

    [Header("Falling Gravity")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private bool fallGravity;
    
    [Header("Wall Jump")]
    [SerializeField] private Vector2 wallJumpDir;

    [Header("Wall Slide")]
    [SerializeField] private float slideSpeed;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;

    [Header("Player Flags")]
    [SerializeField] private bool canMove;
    [SerializeField] private bool groundTouch;
    [SerializeField] private bool wallJumped;
    [SerializeField] private bool wallGrabbed;
    [SerializeField] private bool wallSlide;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool hasDashed;


    // jump~~~~~~~~~~
    // wall grab
    // wall slide
    // dash
    // wall dash

    void Update()
    {
        FallGravityModifier();
        HandleInputs();

        
        Walk(walkDirection);

        if(colScript.onWall && Input.GetKey(wallGrabKey) && canMove)
        {
            wallGrabbed = true;
            wallSlide = false;
        }

        if(Input.GetKeyUp(wallGrabKey) || !colScript.onWall || !canMove)
        {
            wallGrabbed = false;
            wallSlide = false;
        }

        if(colScript.onGround && !isDashing)
        {
            wallJumped = false;
            fallGravity = true;
        }

        if(wallGrabbed && !isDashing)
        {
            rb.gravityScale = 0;
            if(xInput > 0.2f || xInput < -0.2f)
            {
                rb.velocity = new Vector2(rb.velocity.x , 0);
            }
            rb.velocity = new Vector2(rb.velocity.x, yInput * walkSpeed);
        }
        else
        {
            rb.gravityScale = 3;
        }

        if(colScript.onWall && !colScript.onGround)
        {
            if(xInput != 0 && !wallGrabbed)
            {
                wallSlide = true;
                WallSlide();
            }
        }
        

        if(Input.GetKeyDown(jumpKey))
        {
            if(colScript.onGround)
            {
                Jump(Vector2.up, false);
            }
            if(colScript.onWall && !colScript.onGround)
            {
                WallJump();
            }
        }

        if(Input.GetKeyDown(dashKey) && !hasDashed)
        {
            if(xRawInput != 0 || yRawInput != 0)
            {
                Dash(xRawInput, yRawInput);
            }
        }

        if(colScript.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }
        if(!colScript.onGround && groundTouch)
        {
            groundTouch = false;
        }
    }

    private void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;
    }

    private void Dash(float x, float y)
    {
        hasDashed = true;

        rb.velocity = Vector2.zero;
        Vector2 direction = new Vector2(x, y);

        rb.velocity += direction.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    private IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, 0.8f, RigidbodyDrag);

        rb.gravityScale = 0;
        fallGravity = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(0.3f);

        rb.gravityScale = 3;
        fallGravity = true;
        wallJumped = false;
        isDashing = false;
    }

    private IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(0.15f);
        if(colScript.onGround)
        {
            hasDashed = false;
        }
    }

    private void FallGravityModifier()
    {
        if(fallGravity)
        {
            if(rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
    private void HandleInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        xRawInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxis("Vertical");
        yRawInput = Input.GetAxisRaw("Vertical");
        walkDirection = new Vector2(xInput, yInput);
    }

    private void WallJump()
    {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = colScript.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    private void WallSlide()
    {
        if(!canMove)
        {
            return;
        }
        float push;
        bool pushingWall = false;
        if((rb.velocity.x > 0 && colScript.onRightWall) || (rb.velocity.x < 0 && colScript.onLeftWall))
        {
            pushingWall = true;
        }
        if(pushingWall)
        {
            push = 0;
        }
        else
        {
            push = rb.velocity.x;
        }

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Walk(Vector2 direction)
    {
        if(!canMove)
        {
            return;
        }

        if(wallGrabbed)
        {
            return;
        }

        if(!wallJumped)
        {
            rb.velocity = new Vector2(direction.x * walkSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(direction.x * walkSpeed, rb.velocity.y)), 10f * Time.deltaTime);
        }
        
    }    

    private void Jump(Vector2 jumpDirection, bool onWall)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;
    }
    /*
    private void CheckWhichWallGrabbing()
    {
        if(colScript.onRightWall)
        {
            wallJumpDir = Vector2.left;
        }
        else if(colScript.onLeftWall)
        {
            wallJumpDir = Vector2.right;
        }
        //StopCoroutine(DisableMovement(0));
        //StartCoroutine(DisableMovement(0.3f));
    }
    */

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    private IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
