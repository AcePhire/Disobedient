using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowCollider : MonoBehaviour
{
    public bool onBush;
    
    private void OnTriggerStay2D(Collider2D other) {
        //if inside the bush destroy
        if (other.name == "bush"){
            GameObject.Destroy(gameObject);
        }
    }
}
