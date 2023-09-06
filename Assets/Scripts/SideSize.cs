using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SideSize : MonoBehaviour
{

    private int size = 25;

    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.name == "Length"){
            size = Manager.dimensions[0];
        }else if (transform.name == "Width"){
            size = Manager.dimensions[1];
        }

        if (size > 60){
            size = 60;
        }else if (size < 25){
            size = 25;
        }
        
        string sizeText = size.ToString();

        if (size < 10){
            sizeText = "0" + sizeText;
        }

        text.SetText(sizeText);
    }
}
