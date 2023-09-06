using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonBehavior : MonoBehaviour
{
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
        if (gameObject.name == "ExitButton"){
            Application.Quit();
        }else if (gameObject.name == "PlayButton"){
            SceneManager.LoadScene(1);
        }else if (gameObject.name == "BackButton"){
            SceneManager.LoadScene(0);
        }else if (gameObject.name == "RandomButton"){
            //randomize the dimensions
            Manager.randomDimensions();
        }else if (gameObject.name == "StartButton"){
            SceneManager.LoadScene(3);
        }else if (gameObject.name == "HelpButton"){
            SceneManager.LoadScene(2);
        }
    }

    private void OnMouseExit() {
        animator.SetBool("Mouse Over", false);
    }
}
