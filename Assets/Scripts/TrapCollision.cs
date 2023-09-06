using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCollision : MonoBehaviour
{

    private Rigidbody2D rb;
    private TrapMovement trapMovement;

    // Start is called before the first frame update
    void Awake()
    {
        rb = transform.GetComponentInParent<Rigidbody2D>();
        trapMovement = transform.GetComponentInParent<TrapMovement>();
    }  

    private void OnCollisionEnter2D(Collision2D other) {
        //rebound
        if (other.gameObject.tag == "WallY"){
            rb.velocity = new Vector2(trapMovement.xVel*-1f, trapMovement.yVel);
        }else if (other.gameObject.tag == "WallX"){
            rb.velocity = new Vector2(trapMovement.xVel, trapMovement.yVel*-1f);
        }

        if (other.gameObject.tag == "Hill"){
            rb.velocity = new Vector2(trapMovement.xVel, trapMovement.yVel*-1f);
        }
    }
}
