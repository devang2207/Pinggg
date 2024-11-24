[System.Serializable]
public class HighScoreElement 
{
    public string playerName;
    public int score;
    public HighScoreElement(string _playerName, int _score)
    {
        playerName = _playerName;
        score = _score;
    }
}
