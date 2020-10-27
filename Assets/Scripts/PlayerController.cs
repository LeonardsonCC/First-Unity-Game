using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2d;

    // Speed
    public float speed = 1;

    // Ground
    private bool isGrounded = false;
    public Transform isGroundedChecker; 
    public float checkGroundRadius; 
    public LayerMask groundLayer;
    public float rememberGroundedFor; 
    float lastTimeGrounded;

    // Jump
    public float jumpForce = 2;
    public float fallMultiplier = 2.5f; 
    public float lowJumpMultiplier = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        BetterJump();
        CheckIfGrounded();
    }

    void Jump() 
    {
        if (Input.GetKeyDown("up") && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor)) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
    }
    
    void BetterJump()
    {
        if (rb2d.velocity.y < 0) {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb2d.velocity.y > 0 && !Input.GetKey("up")) {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        } 
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        rb2d.velocity = new Vector2(moveBy, rb2d.velocity.y);
    }

    void CheckIfGrounded() 
    {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer); 
        if (colliders != null) { 
            isGrounded = true; 
        } else { 
            if (isGrounded) { 
                lastTimeGrounded = Time.time; 
            } 
            isGrounded = false; 
        }    
    }
}
