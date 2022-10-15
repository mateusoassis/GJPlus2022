using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManagerScript;

    [Header("Tipo de movimentação")]
    private float xInput;
    private float yInput;
    [SerializeField] private Vector2 direction;
    [SerializeField] private bool xRawMovement;
    [SerializeField] private bool yRawMovement;
    

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float playerSpeed;

    [Header("Falling Gravity")]
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        FallGravityModifier();
    }

    private void FallGravityModifier()
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

    private void MovementInput()
    {
        if(yRawMovement)
        {
            yInput = Input.GetAxisRaw("Vertical");
        }
        else
        {
            yInput = Input.GetAxis("Vertical");
        }

        if(xRawMovement)
        {
            xInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            xInput = Input.GetAxis("Horizontal");
        }
        direction = new Vector2(xInput, yInput);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpVelocity;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(direction.x * playerSpeed, rb.velocity.y);
    }
}
