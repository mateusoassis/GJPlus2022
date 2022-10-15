using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecking : MonoBehaviour
{
    public bool seeCollisionGizmos;
    [SerializeField] private Vector2 bottomOffset, rightOffset, leftOffset;
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    [SerializeField] private float collisionRadius;
    [SerializeField] private LayerMask groundLayer;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer); 
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        if(onRightWall || onLeftWall)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[]{bottomOffset, rightOffset, leftOffset};

        if(seeCollisionGizmos)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        }
    }
}
