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
    [SerializeField] private LayerMask normalGroundLayer;
    [SerializeField] private LayerMask grabbableGroundLayer;

    [Header("Lose Transition")]
    [SerializeField] private GameObject youLoseTransition;
    public bool playerLose;
    [SerializeField] private bool restartGame;
    [SerializeField] private AudioClipManager sfx;
    
    void Awake()
    {
        youLoseTransition.SetActive(false);
    }

    void Start()
    {
        playerLose = false;
        restartGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, normalGroundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, grabbableGroundLayer); 
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, grabbableGroundLayer);
        if(onRightWall || onLeftWall)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        if(playerLose && !restartGame)
        {
            restartGame = true;
            sfx.PlayOneShot("DeathSound");
            sfx.PlayOneShot("LoseSound");
        }
        if(restartGame)
        {
            youLoseTransition.SetActive(true);
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
