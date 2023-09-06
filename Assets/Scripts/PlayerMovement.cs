using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;

    public float multiplier;

    public GameObject arrowIndicator;

    public ScoringSystem scoringSystem;

    public GameObject shield;

    private Rigidbody2D rb;

    private Animator animator;

    private float xVel, yVel;

    private bool startMoving, moving = false;

    private Vector2 nextWallHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if held and mouse up
        if (startMoving && Input.GetMouseButtonUp(0)){
            //find the mouse pos and calculate the difference between the object and the mouse
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 difference = mousePos - (Vector2)transform.position;

            //find the angle
            float angle = Mathf.Atan2(difference.y, difference.x);

            //set the velocity
            xVel = Mathf.Cos(angle+Mathf.PI)*50f*multiplier;
            yVel = Mathf.Sin(angle+Mathf.PI)*31.25f*multiplier;

            rb.velocity = new Vector2(xVel, yVel);

            startMoving = false;
            moving = true;
            animator.SetBool("Moving", moving);

            //hide arrow
            arrowIndicator.SetActive(false);

            scoringSystem.update = true;
        }

        if (Input.GetKey(KeyCode.D)){
            shield.transform.RotateAround(transform.position, Vector3.forward, -400f * Time.deltaTime);
        }else if (Input.GetKey(KeyCode.A)){
            shield.transform.RotateAround(transform.position, Vector3.forward, 400f * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.R) && !GetComponent<PlayerHealth>().savingManager.deathScreen.activeSelf){
            SceneManager.LoadScene(1);
        }

        //get angle
        float velocityAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x);

        //using sin and cos to get it within -1 and 1
        animator.SetFloat("X-Velocity", Mathf.Cos(velocityAngle));
        animator.SetFloat("Y-Velocity", Mathf.Sin(velocityAngle));
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && !moving){
            startMoving = true;
        }
    }

    private void OnMouseDrag(){
        if (moving) return;
        //find the mouse pos and calculate the difference between the object and the mouse
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 difference = mousePos - (Vector2)transform.position;

        //find the angle
        float angle = Mathf.Atan2(difference.y, difference.x);

        //rotate the arrow
        arrowIndicator.transform.rotation = Quaternion.Euler(0f, 0f, ((angle+Mathf.PI)*Mathf.Rad2Deg)-90);

        //find distance between mouse and object
        float distance = Mathf.Sqrt(Mathf.Pow(difference.x, 2) + Mathf.Pow(difference.y, 2));

        Transform arrow = arrowIndicator.transform.GetChild(0);

        //max size of yScale * the distance / 15(max distance it can be) so yScale times a number between 0 and 1
        float maxYScale = 0.03568667f;

        float yScale = maxYScale*(distance/15);

        //if its more than max clamp it, if its less cancel action
        if (yScale >= maxYScale) yScale = maxYScale;
        else if (yScale <= 0.008f){
            //hide arrow
            arrowIndicator.SetActive(false);
            startMoving = false;
        }
        else{
            //show arrow
            arrowIndicator.SetActive(true);
            startMoving = true;
        }

        //set the scale
        arrow.localScale = new Vector2(arrow.localScale.x, yScale);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Hill"){
            yVel *= -1f;
            rb.velocity = new Vector2(xVel, yVel);
        }

        //alter direction a little
        if (other.gameObject.name == "bush"){
            float r1 = Random.Range(0.99f, 1.01f);
            float r2 = Random.Range(0.99f, 1.01f);
            rb.velocity *= new Vector2(r1, r2);
        }
    }

    public void Stop(){
        rb.velocity = Vector2.zero;
        xVel = 0;
        yVel = 0;
        animator.SetBool("Moving", false);
    }
}
