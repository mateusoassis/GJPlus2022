using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManagerScript;
    [SerializeField] private CollisionChecking colScript;
    [SerializeField] private StaminaHandler stamHandler;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClipManager audio;

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
    [SerializeField] private float targetGravity;
    
    [Header("Wall Jump")]
    [SerializeField] private Vector2 wallJumpDir;

    [Header("Wall Slide")]
    [SerializeField] private float slideSpeed;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;

    [Header("LocalScale")]
    [SerializeField] private Vector3 localScaleSaved;

    [Header("Player Flags")]
    [SerializeField] private bool canMove;
    [SerializeField] private bool groundTouch;
    [SerializeField] private bool wallJumped;
    [SerializeField] private bool wallGrabbed;
    [SerializeField] private bool wallSlide;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool hasDashed;
    [SerializeField] private bool jumping;
    [SerializeField] private bool facingRight;

    void Start()
    {
        localScaleSaved = anim.gameObject.transform.localScale;
    }

    void Update()
    {
        FallGravityModifier();
        HandleInputs();
        HandleAnimations();
        HandleFlipXScale();
        
        Walk(walkDirection);

        if(colScript.onWall && Input.GetKey(wallGrabKey) && canMove)
        {
            if(stamHandler.currentStamina > 0)
            {
                wallGrabbed = true;
                anim.SetBool("idle", false);
                anim.SetBool("walking", false);
                anim.SetBool("wallgrab", true);
                anim.SetBool("jumping", false);
                anim.SetBool("dashing", false);
                wallSlide = false;
                jumping = false;
                stamHandler.StartDegen();
            }
            else
            {
                wallGrabbed = false;
            }

        }

        if((Input.GetKeyUp(wallGrabKey) || !colScript.onWall || !canMove))
        {
            wallGrabbed = false;
            wallSlide = false;
            stamHandler.StopDegen();
            anim.SetBool("wallgrab", false);
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
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        else
        {
            rb.gravityScale = targetGravity;
        }

        if(colScript.onWall && !colScript.onGround)
        {
            if(xInput != 0 && !wallGrabbed)
            {
                wallSlide = true;
                jumping = false;
                WallSlide();
            }
        }
        

        if(Input.GetKeyDown(jumpKey))
        {
            if(colScript.onGround)
            {
                Jump(Vector2.up, false);
                jumping = true;
                wallGrabbed = false;
                wallSlide = false;
                stamHandler.StopDegen();
            }
            if(colScript.onWall && !colScript.onGround)
            {
                WallJump();
                jumping = true;
            }
        }

        if(Input.GetKeyDown(dashKey) && !hasDashed && (colScript.onGround || colScript.onWall))
        {
            if(stamHandler.dashCost < stamHandler.currentStamina)
            {
                if(xRawInput != 0 || yRawInput != 0)
                {
                    Dash(xRawInput, yRawInput, true);
                    stamHandler.CastDash();
                }
                else
                {
                    if(facingRight)
                    {
                        Dash(1,0, true);
                        stamHandler.CastDash();
                    }
                    else
                    {
                        Dash(-1,0, true);
                        stamHandler.CastDash();
                    }
                } 
            } 
        }

        if(colScript.onGround && !groundTouch)
        {
            GroundTouch();
            if(!groundTouch)
            {
                audio.PlayOneShot("LandSFX");
            }
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
        jumping = false;
        anim.SetBool("falling", false);
    }

    private void Dash(float x, float y, bool dashing)
    {
        hasDashed = true;

        if(dashing == true)
        {
            Debug.Log("dash normal");
            anim.SetBool("idle", false);
            anim.SetBool("walking", false);
            anim.SetBool("wallgrab", false);
            anim.SetBool("jumping", false);
            anim.SetBool("dashing", true);
        }
        else
        {
            Debug.Log("jump da parede");
            anim.SetBool("idle", false);
            anim.SetBool("walking", false);
            anim.SetBool("wallgrab", false);
            anim.SetBool("jumping", true);
            anim.SetBool("dashing", false);
            wallGrabbed = false;
        }
        

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

        rb.gravityScale = targetGravity;
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
        if(xInput == 0 || ((xInput > 0 && !colScript.onLeftWall) || (xInput < 0 && !colScript.onRightWall)))
        {
            if(stamHandler.jumpCost < stamHandler.currentStamina)
            {
                wallJumped = true;
                /*
                anim.SetBool("idle", false);
                anim.SetBool("walking", false);
                anim.SetBool("wallgrab", false);
                anim.SetBool("jumping", true);
                anim.SetBool("dashing", false);
                */
                StopCoroutine(DisableMovement(0));
                StartCoroutine(DisableMovement(.1f));

                //Vector2 wallDir = colScript.onRightWall ? Vector2.left : Vector2.right;

                Jump((Vector2.up / 1.5f/* + wallDir / 1.5f*/), true);
                
                wallJumped = false;
                stamHandler.CastJump();
            }
            
        }
        else if(xInput > 0 && colScript.onLeftWall)
        {
            if(stamHandler.dashCost < stamHandler.currentStamina)
            {
                Dash(1, 1, false);
                stamHandler.CastDash(); 
            } 
           
        }
        else if(xInput < 0 && colScript.onRightWall)
        {
            if(stamHandler.dashCost < stamHandler.currentStamina)
            {
                Dash(-1, 1, false);
                stamHandler.CastDash();
            }
        }
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
        anim.SetBool("idle", false);
        anim.SetBool("walking", false);
        anim.SetBool("wallgrab", false);
        anim.SetBool("jumping", true);
        anim.SetBool("dashing", false);
        if(groundTouch)
        {
            int u = Random.Range(1, 4);
            audio.PlayOneShot("Jump"+u);
        }
    }

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

    private void HandleAnimations()
    {
        if(colScript.onGround)
        {
            if(xInput == 0 && !isDashing && !jumping)
            {
                anim.SetBool("idle", true);
                anim.SetBool("walking", false);
                anim.SetBool("wallgrab", false);
                anim.SetBool("jumping", false);
                anim.SetBool("dashing", false);
            }
            else if(xInput != 0 && !isDashing && !jumping)
            {
                anim.SetBool("idle", false);
                anim.SetBool("walking", true);
                anim.SetBool("wallgrab", false);
                anim.SetBool("jumping", false);
                anim.SetBool("dashing", false);
            }
        }
        else
        {
            if(jumping && rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
            else if(wallGrabbed && !groundTouch && !jumping)
            {
                anim.SetBool("idle", false);
                anim.SetBool("walking", false);
                anim.SetBool("wallgrab", true);
                anim.SetBool("jumping", false);
                anim.SetBool("dashing", false);
            }
            else if(wallJumped && !groundTouch)
            {
                anim.SetBool("idle", false);
                anim.SetBool("walking", false);
                anim.SetBool("wallgrab", false);
                anim.SetBool("jumping", false);
                anim.SetBool("dashing", true);
            }
            if(!jumping && !groundTouch)
            {
                anim.SetBool("idle", false);
                anim.SetBool("walking", false);
                anim.SetBool("wallgrab", false);
                anim.SetBool("jumping", false);
                anim.SetBool("dashing", false);
                anim.SetBool("falling", true);
            }
        }
    }
    private void HandleFlipXScale()
    {
        if((colScript.onGround || !groundTouch || isDashing) && !wallGrabbed)
        {
            if(xInput > 0)
            {
                facingRight = true;
            }
            if(xInput < 0)
            {
                facingRight = false;
            }
            if(facingRight)
            {
                anim.gameObject.transform.localScale = new Vector3(localScaleSaved.x, localScaleSaved.y, localScaleSaved.z);
            }
            else
            {
                anim.gameObject.transform.localScale = new Vector3(-localScaleSaved.x, localScaleSaved.y, localScaleSaved.z);
            }
        }
    }
}
