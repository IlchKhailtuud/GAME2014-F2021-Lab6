using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")] 
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    public Transform groundOrigin; 
    private Rigidbody2D rigidbody;
    public float groundRadius;
    public LayerMask groundLayerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit =
            Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

    private void Move()
    {
        if (isGrounded)
        {
            //Keyboard Input
            var deltaTime = Time.deltaTime;
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            float jump = Input.GetAxisRaw("Jump");

           //Check for flip
           
           if (x != 0)
           {
               x = FlipAnimation(x);
           }

           //Touch Input
            Vector2 worldTouch = new Vector2();
            foreach (var touch in Input.touches)
            {
                worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            float horizontalMoveForce = x * horizontalForce; // * deltaTime;
            float jumpMoveForce = jump * verticalForce; //* deltaTime;

            float mass = rigidbody.mass * rigidbody.gravityScale;
        
            rigidbody.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce));
            rigidbody.velocity *= 0.99f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundOrigin.position, groundRadius);
    }

    private float FlipAnimation(float x)
    {
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);
        return x;
    }
}
