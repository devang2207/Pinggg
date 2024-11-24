using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Score and EndScore")]
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI endSceneScoreTMP;

    [Header("Menus")]
    [SerializeField] private GameObject _endMenuGO;
    [SerializeField] private GameObject _pauseMenuGO;
    [SerializeField] private GameObject _startMenuGO;
    [SerializeField] private GameObject _highScoreGO;

    [Header("TMP InputField Player Name")]
    [SerializeField] public TMP_InputField _nameInput;

    [Header("High Score Elements Reference")]
    [SerializeField] private GameObject highscoreUIElementPrefab;
    [SerializeField] private Transform elementWrapper;

    private List<GameObject> uiElements = new List<GameObject>();

    /// Updates the high score UI list.
    private void UpdateHighScoreUI(List<HighScoreElement> highscoreElementsList)
    {
        // Create UI elements for each high score entry
        for (int i = 0; i < highscoreElementsList.Count; i++)
        {
            HighScoreElement el = highscoreElementsList[i];
            if (el != null)
            {
                GameObject instantiatedPrefab = Instantiate(highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                instantiatedPrefab.transform.SetParent(elementWrapper, false);

                uiElements.Add(instantiatedPrefab);
            }

            // Set player name and score in the UI
            TextMeshProUGUI[] texts = uiElements[i].GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = el.playerName;
            texts[1].text = el.score.ToString();
        }
    }

    /// Initializes all UI menus at the start.
    public void InitializeUI()
    {
        _endMenuGO.SetActive(false);
        _pauseMenuGO.SetActive(false);
        _highScoreGO.SetActive(false);
        _startMenuGO.SetActive(true);
    }


    /// Starts the game by hiding all menus.
    public void StartPlaying()
    {
        _endMenuGO.SetActive(false);
        _pauseMenuGO.SetActive(false);
        _highScoreGO.SetActive(false);
        _startMenuGO.SetActive(false);
    }


    
    // Updates the in-game score display.
    public void UpdateScoreUI(int score)
    {
        if (scoreTMP != null)
            scoreTMP.text = $"SCORE: {score}";
    }

    
    // Displays the end screen with the final score.
    public void ShowEndScreenUI(int score)
    {
        Time.timeScale = 0; // Pause the game
        _endMenuGO.SetActive(true);
        if (endSceneScoreTMP != null)
            endSceneScoreTMP.text = $"SCORE: {score}";
    }
    #region OnClickFunctions
    // Exits the game
    public void QuitGame()
    {
        Application.Quit();
    }
    // Toggles the pause menu visibility.
    public void TogglePauseMenuUI(bool pause)
    {
        _pauseMenuGO.SetActive(pause);
    }

    /// Toggles the high score panel visibility.
    public void ToggleHighScorePanel(bool open)
    {
        _highScoreGO.SetActive(open);
    } 
    #endregion

    #region Subscribe and UnSubscribe to events
    // Subscribes to high score updates
    private void OnEnable()
    {
        HighScoreManager.onHighscoreListChanged += UpdateHighScoreUI;
    }

    // Unsubscribes from high score updates
    private void OnDisable()
    {
        HighScoreManager.onHighscoreListChanged -= UpdateHighScoreUI;
    } 
    #endregion
}
