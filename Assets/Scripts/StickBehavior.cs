using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickBehavior : MonoBehaviour
{
    private bool beingDestroyed, held;

    // Update is called once per frame
    void Update()
    {
        if (held && Input.GetMouseButtonUp(0)){
            held = false;
        }
    }

    //move on drag
    private void OnMouseDrag() {
        held = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //if its on the stick ability and one has let go
        if (other.gameObject.tag == "StickAbility" && !other.GetComponent<StickAbility>().held && held){
            //update the number of abilities left and destroy
            other.GetComponent<StickAbility>().UpdateText(1);

            beingDestroyed = true;

            GameObject.Destroy(gameObject);
        }else{
            //check if its the trap and its not closed and not being destroyed to avoid doing both statements
            if (other.gameObject.tag == "TrapCollider" && !other.GetComponentInParent<Animator>().GetBool("Closed") && !beingDestroyed){
                beingDestroyed = true;

                //play animation and sound
                //if from stick don't reopen
                other.GetComponentInParent<TrapMovement>().fromStick = true;
                other.GetComponentInParent<Animator>().SetBool("Closed", true);
                other.GetComponentInParent<TrapMovement>().PlaySound();
                
                //destroy this
                GameObject.Destroy(gameObject);
            }
        }        
    }
}
