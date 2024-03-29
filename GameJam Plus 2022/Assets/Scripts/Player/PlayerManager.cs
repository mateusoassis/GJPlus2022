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

    [Header("Test Mode")]
    [SerializeField] private bool testing;
    [SerializeField] private PlayerInfo playerInfo;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey1;
    [SerializeField] private KeyCode jumpKey2;
    [SerializeField] private KeyCode wallGrabKey1;
    [SerializeField] private KeyCode wallGrabKey2;
    [SerializeField] private KeyCode dashKey1;
    [SerializeField] private KeyCode dashKey2;

    [Header("Wall Grab Buffer")]
    [SerializeField] private float grabBufferDuration;
    [SerializeField] private float grabBufferTimer;
    [SerializeField] private bool pressedWallGrab;

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
    public int jumpCount;
    public int maxJump;
    
    [Header("Wall Jump")]
    [SerializeField] private Vector2 wallJumpDir;

    [Header("Wall Slide")]
    [SerializeField] private float slideSpeed;

    [Header("Dash")]
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float distanceBetweenImages;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTimeLeft;
    [SerializeField] private float lastImageXpos;
    [SerializeField] private float lastDash;
    

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

    void Awake()
    {
        if(testing)
        {
            playerInfo.checkpointReached = 0;
        }
    }

    void Start()
    {
        localScaleSaved = anim.gameObject.transform.localScale;
        jumpCount = 0;
        grabBufferTimer = grabBufferDuration;
    }

    void Update()
    {
        FallGravityModifier();
        HandleAnimations();
        HandleInputs();
        HandleFlipXScale();
        
        Walk(walkDirection);

        if(((Input.GetKeyUp(wallGrabKey1) || Input.GetKeyUp(wallGrabKey2)) || !colScript.onWall || !canMove) || jumping)
        {
            anim.SetBool("wallgrab", false);
            wallGrabbed = false;
            wallSlide = false;
            stamHandler.StopDegen();
            jumpCount = 0;
        }

        CheckWallGrabBuffer();

        if((Input.GetKeyDown(wallGrabKey1) || Input.GetKeyDown(wallGrabKey2)) && canMove)
        {
            if(!pauseManagerScript.gamePaused)
            {
                Debug.Log("apertou wallgrab");
                if(!pressedWallGrab)
                {
                    Debug.Log("pressedwallgrab = true");
                    pressedWallGrab = true;
                }
            }    
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
        else if(colScript.onGround && !jumping && !colScript.onWall)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = targetGravity;
        }

        if(colScript.onWall && !colScript.onGround)
        {
            if(xInput != 0 && !wallGrabbed)
            {
                jumping = false;
            }
        }
        

        if(Input.GetKeyDown(jumpKey1) || Input.GetKeyDown(jumpKey2))
        {
            if(!pauseManagerScript.gamePaused)
            {
                if(jumpCount < maxJump)
                {
                    if(colScript.onGround)
                    {
                        jumpCount++;
                        Jump(Vector2.up, false);
                        jumping = true;
                        wallGrabbed = false;
                        wallSlide = false;
                        stamHandler.StopDegen();
                    }
                    else if(colScript.onWall && !colScript.onGround && wallGrabbed)
                    {
                        WallJump();
                        jumpCount++;
                        jumping = true;
                    }
                }
            }   
        }

        if((Input.GetKeyDown(dashKey1) || Input.GetKeyDown(dashKey2)) && !hasDashed && (colScript.onGround || colScript.onWall))
        {
            if(!pauseManagerScript.gamePaused)
            {
                if(stamHandler.dashCost < stamHandler.currentStamina)
                {
                    if(xRawInput != 0 || yRawInput != 0)
                    {
                        Dash(xRawInput, 0, true);
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
        }
        CheckDash();

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
        //jumpCount = 0;
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
            audio.PlayOneShot("DashSound");
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

        // Afterimage
        AfterimagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        rb.gravityScale = 0;
        fallGravity = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = targetGravity;
        fallGravity = true;
        wallJumped = false;
        isDashing = false;
    }
    private void CheckDash()
    {
        if(isDashing)
        {
            if(dashTimeLeft > 0)
            {
                dashTimeLeft -= Time.deltaTime;
                if(Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    AfterimagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
        }
    }

    private void CheckWallGrabBuffer()
    {
        if(pressedWallGrab)
        {
            if(grabBufferTimer > 0)
            {
                grabBufferTimer -= Time.deltaTime;
                //wallGrabbed = true;
            }
            else
            {
                DeactivatePressedWallGrabBool();
            }

            
            if(stamHandler.currentStamina > 0 && !wallGrabbed && colScript.onWall)
            {
                //if(!wallGrabbed)
                //{
                    audio.PlayOneShot("WallGrab");
                //}
                wallGrabbed = true;
                DeactivatePressedWallGrabBool();
                
                anim.SetBool("idle", false);
                anim.SetBool("walking", false);
                anim.SetBool("wallgrab", true);
                anim.SetBool("jumping", false);
                anim.SetBool("dashing", false);
                
                //audio.PlayOneShot("WallGrab");
                wallSlide = false;
                jumping = false;
                stamHandler.StartDegen();
            }
            else
            {
                wallGrabbed = false;
                anim.SetBool("wallgrab", false);
                //DeactivatePressedWallGrabBool();
            }
        }
        
    }
    private void DeactivatePressedWallGrabBool()
    {
        pressedWallGrab = false;
        grabBufferTimer = grabBufferDuration;
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
        walkDirection = new Vector2(xRawInput, yRawInput);
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
                //StopCoroutine(DisableMovement(0));
                //StartCoroutine(DisableMovement(.1f));

                //Vector2 wallDir = colScript.onRightWall ? Vector2.left : Vector2.right;

                Jump((Vector2.up / 1.5f/* + wallDir / 1.5f*/), true);

                //anim.SetBool("jumping", true);
                
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
        /* 
        anim.SetBool("idle", false);
        anim.SetBool("walking", false);
        anim.SetBool("wallgrab", false);
        anim.SetBool("jumping", true);
        anim.SetBool("dashing", false);
        */
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
            jumpCount = 0;
            if(xRawInput == 0 && !isDashing && !jumping)
            {
                anim.SetBool("idle", true);
                anim.SetBool("walking", false);
                anim.SetBool("wallgrab", false);
                anim.SetBool("jumping", false);
                anim.SetBool("dashing", false);
            }
            else if(xRawInput != 0 && !isDashing && !jumping)
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
            if(jumping)
            {
                if(rb.velocity.y < 0)
                {
                    anim.SetBool("idle", false);
                    anim.SetBool("walking", false);
                    anim.SetBool("wallgrab", false);
                    anim.SetBool("jumping", false);
                    anim.SetBool("dashing", false);
                    anim.SetBool("falling", true);
                }
                else
                {
                    anim.SetBool("idle", false);
                    anim.SetBool("walking", false);
                    anim.SetBool("wallgrab", false);
                    anim.SetBool("jumping", true);
                    anim.SetBool("dashing", false);
                    anim.SetBool("falling", false);
                }
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
                if(jumping)
                {
                    anim.SetBool("idle", false);
                    anim.SetBool("walking", false);
                    anim.SetBool("wallgrab", false);
                    anim.SetBool("jumping", true);
                    anim.SetBool("dashing", false);
                }
                if(!groundTouch)
                {
                    anim.SetBool("idle", false);
                    anim.SetBool("walking", false);
                    anim.SetBool("wallgrab", false);
                    anim.SetBool("jumping", false);
                    anim.SetBool("dashing", true);
                }  
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
