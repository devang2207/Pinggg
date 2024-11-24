using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Enum to represent different game states
    private enum GameStates
    {
        StartState,
        PlayingState,
        PauseState,
        EndState
    }

    #region Variables And References
    [Header("Other Managers reference")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private HighScoreManager _highScoreHandler;
    [SerializeField] private Ball ballBehaviour;

    private GameStates currentState;
    private string playerName;
    private int score = 0; 
    #endregion

    #region Start And Update
    private void Start()
    {
        EnterStartState();
    }
    private void Update()
    {
        // Handle gameplay state toggles
        switch (currentState)
        {
            case GameStates.PlayingState:
                if (Input.GetKeyDown(KeyCode.Escape)) EnterPauseState();
                break;

            case GameStates.PauseState:
                if (Input.GetKeyDown(KeyCode.Escape)) EnterPlayingState();
                break;
        }
    } 
    #endregion

    #region UI Buttons

    // Triggered when game starts
    // Reloads the current scene
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartGame()
    {
        playerName = uiManager._nameInput.text;
        uiManager.StartPlaying();
        currentState = GameStates.PlayingState;
        Time.timeScale = 1.0f;
    }
    public void PauseGame()
    {
        EnterPauseState();
    }
    public void ResumeGame()
    {
        EnterPlayingState();
    }

    // Exits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Restarts game without resetting player name
    public void RestartGameWithSameName()
    {
        score = 0;
        Time.timeScale = 1.0f;
        uiManager.StartPlaying();
        uiManager.UpdateScoreUI(score);
        currentState = GameStates.PlayingState;
        ballBehaviour.ResetBall();
    } 
    #endregion

    #region Enter GameStates
    // Initializes start state and UI
    private void EnterStartState()
    {
        currentState = GameStates.StartState;
        UpdateScoreUI();
        uiManager.InitializeUI();
        Time.timeScale = 0.0f;
    }
    // Resumes the game from pause
    public void EnterPlayingState()
    {
        currentState = GameStates.PlayingState;
        uiManager.TogglePauseMenuUI(false);
        Time.timeScale = 1.0f;
    }

    // Pauses the game
    private void EnterPauseState()
    {
        currentState = GameStates.PauseState;
        uiManager.TogglePauseMenuUI(true);
        Time.timeScale = 0.0f;
    }

    // Handles game over behavior
    private void EnterEndState()
    {
        currentState = GameStates.EndState;
        Time.timeScale = 0.0f;
        uiManager.ShowEndScreenUI(score);
        _highScoreHandler.AddHighScoreIfPossible(new HighScoreElement(playerName, score));
    }

    // Ends the game
    private void EndGame()
    {
        EnterEndState();
    } 
    #endregion
    // Increases player score
    #region Update Score
    private void IncreaseScore()
    {
        score++;
        UpdateScoreUI();
    }


    // Updates the score UI
    private void UpdateScoreUI()
    {
        uiManager.UpdateScoreUI(score);
    }

    #endregion
    // Subscribe to ball events
    #region Subscribe To Events
    private void OnEnable()
    {
        Ball.OnPlayerScore += IncreaseScore;
        Ball.OnEnemyScore += EndGame;
    }

    // Unsubscribe from ball events
    private void OnDisable()
    {
        Ball.OnPlayerScore -= IncreaseScore;
        Ball.OnEnemyScore -= EndGame;
    } 
    #endregion
}
