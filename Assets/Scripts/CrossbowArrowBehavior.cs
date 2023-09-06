using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CrossbowArrowBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other is TilemapCollider2D){
            GameObject.Destroy(gameObject);
        }

        if (other.gameObject.tag == "Shield"){
            GameObject.Destroy(gameObject);
        }
    }
}
