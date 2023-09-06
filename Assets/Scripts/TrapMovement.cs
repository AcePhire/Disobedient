using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMovement : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject deathScreen;

    public float xVel, yVel;
    public float closedTimer = 0;

    public bool fromStick = false;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private Vector2 mousePos1;

    private float holdingTimer = 0;
    private bool held = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //make sure we pressed on it
        if (held){
            //countdown
            holdingTimer -= Time.deltaTime;

            //check if time's up and release
            if (holdingTimer <= 0){
                Push();
            }else{
                if (Input.GetMouseButtonUp(0)){
                    Push();
                 }
            }
        }

        //if the trap got closed, wait 5 secs and reopen it
        if (animator.GetBool("Closed") && !fromStick){
            closedTimer -= Time.deltaTime;

            if (closedTimer <= 0){
                animator.SetBool("Closed", false);
            }
        }

        //save veclocity
        xVel = rb.velocity.x;
        yVel = rb.velocity.y;
        
        //alter velocity lowering it to zero
        if (xVel <= -1 || xVel >= 1){
            if (yVel <= -1 || yVel >= 1){
                rb.velocity = rb.velocity*0.95f;
            }else rb.velocity = Vector2.zero;
        }else rb.velocity = Vector2.zero;
    }

    private void OnMouseDrag() {
        //Move object to mouse if not moving and on cam
        if (held && rb.velocity.x == 0 && rb.velocity.y == 0 && OnScreen() && !deathScreen.activeSelf){
            transform.position = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);   
        }
    }

    private void OnMouseOver() {
        //cant hold while moving and while off cam
        if (Input.GetMouseButtonDown(0) && rb.velocity.x == 0 && rb.velocity.y == 0 && OnScreen() && !deathScreen.activeSelf){
            //save on click pos
            mousePos1 = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //reset velocity to cancel any other velocity
            rb.velocity = Vector2.zero;
            //reset timer
            holdingTimer = 0.5f;
            held = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        //if inside the bush destroy
        if (other.name == "bush"){
            GameObject.Destroy(gameObject);
        }
    }

    //make sure the object is on the screen
    private bool OnScreen(){
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        if (GeometryUtility.TestPlanesAABB(planes, transform.GetChild(0).GetComponent<Collider2D>().bounds)){
            return true;
        }else{
            return false;
        }
    }

    private void Push(){
        held = false;
        Vector2 difference = (Vector2)transform.position - mousePos1;

        //dont push if it didnt move
        if(difference != Vector2.zero){
            //get angle of drag
            float angle = Mathf.Atan2(difference.y, difference.x);

            float pushForce = 5000;

            //push it
            rb.AddForce(new Vector2(Mathf.Cos(angle) * pushForce, Mathf.Sin(angle) * pushForce));
        }
    }

    public void PlaySound(){
        audioSource.Play();
    }
}
