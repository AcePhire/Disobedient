using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    public int changeBy;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseEnter() {
        animator.SetBool("Mouse Over", true);
    }

    private void OnMouseDown() {
        //if its length change it in dimension[0] by either 1, 10, 100 or -1, -10, -100
        if (transform.parent.name == "Length"){
            int size = Manager.dimensions[0];
            size += changeBy;
            size = Mathf.Clamp(size, 20, 60);
            Manager.dimensions[0] = size;
        //if its length change it in dimension[1] by either 1, 10, 100 or -1, -10, -100
        }else if (transform.parent.name == "Width"){
            int size = Manager.dimensions[1];
            size += changeBy;
            size = Mathf.Clamp(size, 20, 60);
            Manager.dimensions[1] = size;
        }
    }

    private void OnMouseExit() {
        animator.SetBool("Mouse Over", false);
    }
}
