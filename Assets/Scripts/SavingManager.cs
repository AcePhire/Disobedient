using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData{
    public int highScore = 0;
}

public class SavingManager : MonoBehaviour
{
    public GameObject deathScreen;

    public GameObject stickAbility;

    public ScoringSystem scoringSystem;

    // Start is called before the first frame update
    void Start()
    {
        string json;

        try{
           json = File.ReadAllText(Application.dataPath + "/json/score.json");
        }catch(System.Exception e){
            if (e is DirectoryNotFoundException){
                Directory.CreateDirectory(Application.dataPath + "/json");

                File.WriteAllText(Application.dataPath + "/json/score.json", JsonUtility.ToJson(new SaveData()));
            }
            
            if (e is FileNotFoundException){
                File.WriteAllText(Application.dataPath + "/json/score.json", JsonUtility.ToJson(new SaveData()));
            }

            json = File.ReadAllText(Application.dataPath + "/json/score.json");
        }
        
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        scoringSystem.highScore.SetText(data.highScore.ToString());
    }
    
    public void endGame(){
        //disable the UI, stop the score, and show death screen
        deathScreen.SetActive(true);
        stickAbility.SetActive(false);

        scoringSystem.update = false;

        //load the json and get the high score
        string loadedJson = File.ReadAllText(Application.dataPath + "/json/score.json");

        SaveData loadedData = JsonUtility.FromJson<SaveData>(loadedJson);

        int highScore = loadedData.highScore;

        if (scoringSystem.time > highScore){
            SaveData savedData = new SaveData();
            savedData.highScore = (int)scoringSystem.time;

            string savedJson = JsonUtility.ToJson(savedData);
            
            File.WriteAllText(Application.dataPath + "/json/score.json", savedJson);
        }
    }
}


