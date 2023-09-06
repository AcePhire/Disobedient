using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoringSystem : MonoBehaviour
{

    public TextMeshProUGUI scoringNumber;

    public TextMeshProUGUI highScore;

    public float time;

    public bool update = false;

    // Update is called once per frame
    void Update()
    {
        if (update){
            time += Time.deltaTime;

            scoringNumber.SetText(((int)(time)).ToString());
        }
    }
}
