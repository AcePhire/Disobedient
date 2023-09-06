using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = transform.parent.GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "TrapCollider" && !other.GetComponentInParent<Animator>().GetBool("Closed")){
            playerHealth.takeDamage(20);
            other.GetComponentInParent<Animator>().SetBool("Closed", true);
            other.GetComponentInParent<TrapMovement>().closedTimer = 5f;
            other.GetComponentInParent<TrapMovement>().PlaySound();
        }

        if (other.gameObject.tag == "Arrow"){
            playerHealth.takeDamage(20);
            GameObject.Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Hill"){
            other.GetComponent<HillBehavior>().playerOver = true;
        }
    }
}
