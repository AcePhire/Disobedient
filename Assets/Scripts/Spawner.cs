using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a class that stores the points of an area
[System.Serializable]
public class Area{
    public Vector2[] areaPoints = new Vector2[4];

    public Area(Vector2[] areaPoints){
        this.areaPoints = areaPoints;
    }

    public bool withinXs(Vector2 point){
        if (point.x >= areaPoints[0].x && point.x <= areaPoints[1].x){
            return true;
        }

        return false;
    }
    public bool withinYs(Vector2 point){
        if (point.y <= areaPoints[0].y && point.y >= areaPoints[2].y){
            return true;
        }

        return false;
    }
    
    public float getRandomRangeX(){
        Vector2 point1 = areaPoints[0];
        Vector2 point2 = areaPoints[1];

        return Random.Range(point1.x, point2.x);
    }

    public float getRandomRangeY(){
        Vector2 point1 = areaPoints[0];
        Vector2 point2 = areaPoints[2];

        return Random.Range(point1.y, point2.y);
    }
}

public class Spawner : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject mapGenerator;
    public GameObject deathScreen;
    public GameObject player;
    public GameObject[] trapTypes;
    public GameObject stick;

    public GameObject playerCollider;

    public ScoringSystem scoringSystem;

    public Area spawningArea;
    public List<Area> restrictedAreas = new List<Area>();

    public GameObject logDisplay;

    private int trapCount;

    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        //set camera area and add it to the restricted areas
        float cameraHeight = 2*mainCamera.orthographicSize;
        float cameraWidth = cameraHeight*mainCamera.aspect;
        restrictedAreas.Add(new Area(new Vector2[]
        {
            new Vector2(-cameraWidth/2, cameraHeight/2), 
            new Vector2(cameraWidth/2, cameraHeight/2), 
            new Vector2(-cameraWidth/2, -cameraHeight/2), 
            new Vector2(cameraWidth/2, -cameraHeight/2)
        }));

        int mapArea = mapGenerator.GetComponent<MapGenerator>().length * mapGenerator.GetComponent<MapGenerator>().width;
        trapCount = mapArea/45;

        for (int i = 0; i < trapCount; i++){
            Vector2 trapPos = new Vector2(spawningArea.getRandomRangeX(), spawningArea.getRandomRangeY());

            bool isInRestrictedArea = false;
            
            //go thro each restricted area and make sure it's not in it
            foreach (Area area in restrictedAreas){
                if (area.withinXs(trapPos) && area.withinYs(trapPos)){
                    isInRestrictedArea = true;
                }
            }

            //if its in a restricted area then repeat this loop
            if (isInRestrictedArea) i--;
            //else make the trap
            else{
                GameObject trap = trapTypes[0];

                GameObject clone = Instantiate(trap, trapPos, Quaternion.identity);
                clone.transform.SetParent(transform);
                clone.GetComponent<TrapMovement>().deathScreen = deathScreen;
                clone.GetComponent<TrapMovement>().mainCamera = mainCamera;

                Physics2D.IgnoreCollision(clone.transform.GetChild(0).GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
            }
        }

        for (int i = 0; i < trapCount/10; i++){
                Vector2 objectPos = new Vector2(spawningArea.getRandomRangeX(), spawningArea.getRandomRangeY());
                GameObject clone = Instantiate(stick, objectPos, Quaternion.identity);

                bool isWithinTrap = false;

                for (int j = 0; j < transform.childCount; j++){
                    if (transform.GetChild(j).GetComponentInChildren<BoxCollider2D>().bounds.Intersects(clone.GetComponent<BoxCollider2D>().bounds)){
                        isWithinTrap = true;
                    }
                }

                if (isWithinTrap){
                    GameObject.Destroy(clone);
                    i--;
                } 
                
            }
    }
    
    // Update is called once per frame
    void Update(){
        //if the number of children is less than the amount of traps wanted then keeping creating traps until it has reached
        //the number of children will be lowered because they will be inside the walls of the map so they are destroyed
        if (transform.childCount < trapCount){
            for (;transform.childCount < trapCount;){
                Vector2 trapPos = new Vector2(spawningArea.getRandomRangeX(), spawningArea.getRandomRangeY());

                bool isInRestrictedArea = false;
                
                //go thro each restricted area and make sure it's not in it
                foreach (Area area in restrictedAreas){
                    if (area.withinXs(trapPos) && area.withinYs(trapPos)){
                        isInRestrictedArea = true;
                    }
                }

                //if its in a restricted area then repeat this loop
                if (!isInRestrictedArea){
                    GameObject trap = trapTypes[0];

                    GameObject clone = Instantiate(trap, trapPos, Quaternion.identity);
                    clone.transform.SetParent(transform);
                    clone.GetComponent<TrapMovement>().deathScreen = deathScreen;
                    clone.GetComponent<TrapMovement>().mainCamera = mainCamera;

                    Physics2D.IgnoreCollision(clone.transform.GetChild(0).GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
                }
            }
        }

        if (scoringSystem.time - score >= 15){
            score = (int)scoringSystem.time;

            Vector2 trapPos = new Vector3(spawningArea.getRandomRangeX(), spawningArea.getRandomRangeY(), 0);

            bool isInRestrictedArea = false;
            
            //go thro each restricted area and make sure it's not in it
            foreach (Area area in restrictedAreas){
                if (area.withinXs(trapPos) && area.withinYs(trapPos)){
                    isInRestrictedArea = true;
                }
            }

            if (!isInRestrictedArea){
                GameObject trap = trapTypes[1];

                GameObject clone = Instantiate(trap, trapPos, Quaternion.identity);
                clone.transform.SetParent(transform);
                clone.GetComponent<CrossbowBehavior>().playerColliderObject = playerCollider;
                clone.GetComponent<CrossbowBehavior>().scoringSystem = scoringSystem;

                logDisplay.SetActive(true);
            }
        }
    }
    
}


