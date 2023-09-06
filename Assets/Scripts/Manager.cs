using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static int[] dimensions = new int[] {20, 20};

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void randomDimensions(){
        dimensions[0] = Random.Range(20, 60);
        dimensions[1] = Random.Range(20, 60);
    }
}
