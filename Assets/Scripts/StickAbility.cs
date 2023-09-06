using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickAbility : MonoBehaviour
{
    public Text text;

    public GameObject stick;

    public int amount = 3;

    private Vector2 oPos;

    public bool held;

    // Start is called before the first frame update
    void Start()
    {
        //update text
        UpdateText(0);
    }

    // Update is called once per frame
    void Update()
    {
        //if mouse up and was held
        if (held && Input.GetMouseButtonUp(0)){
            //create a stick in its place
            GameObject clone = Instantiate(stick);
            clone.transform.position = transform.position;
            transform.localPosition = oPos;

            //use one of the abilities and update text
            UpdateText(-1);

            held = false;
        }
    }

    //update the text of the amount
    public void UpdateText(int change){
        amount += change;
        text.text = amount.ToString();
    }

    private void OnMouseOver() {
        //once press, save starting pos
        if (Input.GetMouseButtonDown(0)){
            oPos = transform.localPosition;
        }
    }

    private void OnMouseDrag() {
        //if have enough abilities, drag and set held
        if (amount > 0){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
            held = true;
        }
    }
}
