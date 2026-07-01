using UnityEngine;
using System.IO; //We need to work with the files,saving on the disk

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; // The static variable "Instance" stores a reference to a single instance of this class

    public string PlayerName; // Data to save between scenes,player's current name
    public string LastPlayerName; //The last name entered for auto-suggestion


    // Data to save between sessions
    public string HighScorePlayerName;
    public int HighScore;

    [System.Serializable]
    class SaveData
    {
        public string HighScorePlayerName;
        public int HighScore;
        public string LastPlayerName; // Adding the field to the JSON structure
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.HighScorePlayerName = HighScorePlayerName;
        data.HighScore = HighScore;
        data.LastPlayerName = LastPlayerName; // Writing down the last name before saving

        // Turning the object into a JSON format string
        string json = JsonUtility.ToJson(data);

        // Writing a line to a file
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) // if we've played before so if the data has been already saved
        {
            string json = File.ReadAllText(path);

            //Turning the JSON back into a SaveData object
           SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScorePlayerName = data.HighScorePlayerName;
            HighScore = data.HighScore;
            LastPlayerName = data.LastPlayerName;// Uploading the last name
        }
    }

  

    private void Awake()
    {
        // Singleton logic: if an instance already exists, we destroy the new one (so that there are no duplicates)
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);// This object will survive the change of scenes

        LoadHighScore(); // When starting the game, we immediately try to load the saved record
    }

    public void ResetData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        // If the file exists on the disk, deleting it
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        // Reset variables in the game itself right now

        HighScore = 0;
        HighScorePlayerName = "";
        LastPlayerName = "";
        PlayerName = "";
    }

}
