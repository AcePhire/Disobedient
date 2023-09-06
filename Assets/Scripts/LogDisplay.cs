using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDisplay : MonoBehaviour
{
    private float timer = 5f;
    public float remainingTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0){
            gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        remainingTime = timer;
    }
}
