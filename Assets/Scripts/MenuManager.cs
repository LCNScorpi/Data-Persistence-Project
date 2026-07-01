using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor; // It is necessary for the Quit button to work in the Unity editor itself 
#endif

public class MenuManager : MonoBehaviour
{

    public TMP_InputField nameInputField; // The link to the input field that we will drag in the inspector
    public TextMeshProUGUI bestScoreText; // Link to the record text in the Main Menu

    private void Start()
    {
        //If there is a record, we display it on the screen in the menu.
        if (DataManager.Instance != null && DataManager.Instance.HighScore > 0)
        {
            bestScoreText.text = $"Best Score : {DataManager.Instance.HighScorePlayerName} : {DataManager.Instance.HighScore}";
        }

        else
        {
            bestScoreText.text = "Best Score : 0";
        }

        //Auto-substitution of a name from a previous session
        if (DataManager.Instance != null && !string.IsNullOrEmpty(DataManager.Instance.LastPlayerName))
        {
            nameInputField.text = DataManager.Instance.LastPlayerName;
        }
    }

    // We will put this function on the Start button
    public void StartGame()
    {
        // 1. We take the text from the UI and save it to our Singleton
        // We save the name both as the current one and as the "last entered" one
        DataManager.Instance.PlayerName = nameInputField.text;
        DataManager.Instance.LastPlayerName = nameInputField.text;

        // Force saving to disk so that the name is written to JSON immediately when you click Start
        DataManager.Instance.SaveHighScore();

        // Loading the game scene
        SceneManager.LoadScene(1);
    }

    public void ResetButtonTriggered()
    {
        if (DataManager.Instance != null)
        {
            //Delete the files and reset the variables
            DataManager.Instance.ResetData();

            //We immediately update the text and input on the screen so that the player can see the changes.
            bestScoreText.text = "Best Score : 0 ";
            nameInputField.text = "";
        }
    }

    // We will put this function on the Quit button
    public void ExitGame()
    {
        //Just in case, we keep the record before quitting
        DataManager.Instance.SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#else 
        Application.Quit();
#endif
    }
}
