using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillBehavior : MonoBehaviour
{

    public GameObject player;

    public bool playerOver;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        //if was on the hill and it's above it now then
        if (playerOver && player.transform.position.y > transform.position.y + 5){
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
            playerOver = false;
        //if it was above it and went under it without hitting it then
        }else if (!playerOver && player.transform.position.y < transform.position.y - 5){
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
        }
    }
}
