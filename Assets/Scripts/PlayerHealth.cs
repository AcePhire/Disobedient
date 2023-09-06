using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth;

    public HealthBar healthBar;

    public SavingManager savingManager;

    public TextMeshProUGUI restartTimerText;

    private int currentHealth;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update(){
        if (currentHealth <= 0 && timer == 0){
            GetComponent<PlayerMovement>().Stop();
            savingManager.endGame();
            timer = 5;
        }

        if (timer > 0){
            timer -= Time.deltaTime;
            restartTimerText.SetText(Mathf.FloorToInt(timer + 1).ToString());
        }

        if (int.Parse(restartTimerText.text) == 0) SceneManager.LoadScene(1);

    }

    public void takeDamage(int damage){
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }
}
