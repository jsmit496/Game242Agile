using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text;

public class LevelData : MonoBehaviour
{
    public GameObject player;

    DataHandler dataHandler;
    LevelController levelController;

	// Use this for initialization
	void Start ()
    {
        string[] fileNames = System.IO.Directory.GetFiles(".", "*.lvl");

        StringBuilder sb = new StringBuilder();
        foreach (string fileName in fileNames)
        {
            sb.AppendLine(fileName);
        }
        print(sb.ToString());

        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void LoadScene()
    {
        //SceneManager.LoadScene("SampleScene");
    }

    public void LoadLevel(string levelName)
    {
        //SceneManager.LoadScene("SampleScene");
        DataHandler level = DataHandler.LoadFromFile(levelName + ".lvl");
        levelController.levelName = level.levelName;
        player.transform.position = level.PlayerPosition;
    }

    public void SaveLevel()
    {
        DataHandler level = new DataHandler();
        level.levelName = levelController.levelName;
        level.PlayerPosition = levelController.playerPosition;

        level.SaveToFile(level.levelName + ".lvl");
    }
}

[Serializable]
public class DataHandler
{
    public string levelName;

    public Vector3 playerPosition;

    public string GetLevelName()
    {
        return levelName;
    }

    public Vector3 PlayerPosition
    {
        get
        {
            return playerPosition;
        }
        set
        {
            playerPosition = value;
        }
    }

    public void SaveToFile(string fileName)
    {
        System.IO.File.WriteAllText(fileName, JsonUtility.ToJson(this, true));
        MonoBehaviour.print(System.IO.Directory.GetCurrentDirectory());
    }

    public static DataHandler LoadFromFile(string fileName)
    {
        return JsonUtility.FromJson<DataHandler>(System.IO.File.ReadAllText(fileName));
    }
}
