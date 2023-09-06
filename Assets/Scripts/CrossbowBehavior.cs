using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowBehavior : MonoBehaviour
{

    public CircleCollider2D reachableAttackRange;
    public CircleCollider2D unreachableAttackRange;

    public ScoringSystem scoringSystem;

    public GameObject barrel;

    public GameObject crossbowBase;

    public GameObject playerColliderObject;

    public GameObject arrow;

    private float timeBetweenShots;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenShots = 1;
    }

    // Update is called once per frame
    void Update()
    {   
        if (!scoringSystem.update) return;
        
        BoxCollider2D playerCollider = playerColliderObject.GetComponent<BoxCollider2D>();

        //check if within range
        if (playerCollider.bounds.Intersects(reachableAttackRange.bounds)){
            // //rotate the barrel
            float arrowSpeed = 100f;

            Vector3 interceptPoint = FirstOrderIntercept(crossbowBase.transform.position, Vector3.zero, arrowSpeed, playerColliderObject.transform.position, playerCollider.transform.GetComponentInParent<Rigidbody2D>().velocity);

            Vector2 difference = interceptPoint - crossbowBase.transform.position;

            float angle = Mathf.Atan2(difference.y, difference.x);

            float angleInDeg = Mathf.Rad2Deg*angle-90;
            
            barrel.transform.RotateAround(crossbowBase.transform.position, Vector3.forward, angleInDeg - barrel.transform.localRotation.eulerAngles.z);


            if (!playerCollider.bounds.Intersects(unreachableAttackRange.bounds)){
                //shoot and wait 5 secs
                if (timeBetweenShots <= 0){
                    GameObject arrowClone = Instantiate(arrow);
                    arrowClone.transform.SetParent(barrel.transform);
                    arrowClone.transform.localPosition = new Vector3(0.9724503f, -0.6f, 0);
                    arrowClone.transform.localScale = new Vector3(0.86f, 0.86f, 1f);
                    arrowClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    arrowClone.transform.parent = null;

                    arrowClone.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * arrowSpeed, Mathf.Sin(angle) * arrowSpeed);

                    timeBetweenShots = 5f;
                }else if (timeBetweenShots > 0) timeBetweenShots -= Time.deltaTime;
            }
        }else timeBetweenShots = 1;
    }

    public Vector3 FirstOrderIntercept(Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity) {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
	    float t = FirstOrderInterceptTime(shotSpeed, targetRelativePosition, targetRelativeVelocity);
        return targetPosition + t*(targetRelativeVelocity);
    }
    //first-order intercept using relative target position
    public float FirstOrderInterceptTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity) {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if(velocitySquared < 0.001f) return 0f;
    
        float a = velocitySquared - shotSpeed*shotSpeed;
    
        // //handle similar velocities
        // if (Mathf.Abs(a) < 0.001f) {
        //     float t = -targetRelativePosition.sqrMagnitude / (2f*Vector3.Dot(targetRelativeVelocity, targetRelativePosition));
        //     return Mathf.Max(t, 0f); //don't shoot back in time
        // }else return 0;
    
        float b = 2f*Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b*b - 4f*a*c;
    
        if (determinant > 0f) { //determinant > 0; two intercept paths (most common)
            float	t1 = (-b + Mathf.Sqrt(determinant))/(2f*a),
                    t2 = (-b - Mathf.Sqrt(determinant))/(2f*a);
            if (t1 > 0f) {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            } else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        } else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b/(2f*a), 0f); //don't shoot back in time
    }
}
