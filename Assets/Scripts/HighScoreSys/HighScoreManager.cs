using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField] string _fileName;
    List<HighScoreElement> _elementsList = new List<HighScoreElement>();
    int _maxHighScore = 5;
    public delegate void OnHighscoreListChanged(List<HighScoreElement> highScoreElementList);
    public static event OnHighscoreListChanged onHighscoreListChanged;

    private void Start()
    {
        LoadHighScores();
    }
    void LoadHighScores()
    {
        _elementsList = FileHandler.ReadListFromJSON<HighScoreElement>(_fileName);
        while (_elementsList.Count > _maxHighScore)
        {
            _elementsList.RemoveAt(_maxHighScore);
        }
        onHighscoreListChanged?.Invoke(_elementsList);
    }
    void SaveHighScore()
    {
        FileHandler.SaveToJSON<HighScoreElement>(_elementsList, _fileName);
    }
    public void AddHighScoreIfPossible(HighScoreElement _highScoreElement)
    {
        for (int i = 0; i < _maxHighScore; i++)
        {
            if (i >= _elementsList.Count || _highScoreElement.score > _elementsList[i].score)
            {
                _elementsList.Insert(i, _highScoreElement);


                while (_elementsList.Count > _maxHighScore)
                {
                    _elementsList.RemoveAt(_maxHighScore);
                }
                SaveHighScore();
                onHighscoreListChanged?.Invoke(_elementsList);
                break;
            }
        }
    }
}
