using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] GameObject highscoreUIElementPrefab;
    [SerializeField] Transform elementWrapper;

    List<GameObject> uiElements = new List<GameObject>();

    private void UpdateUI(List<HighScoreElement> highscoreElementsList)
    {
        for (int i = 0; i < highscoreElementsList.Count; i++)
        {
            HighScoreElement el = highscoreElementsList[i];
            if (el != null)
            {
                GameObject instantiatedPrefab = Instantiate(highscoreUIElementPrefab,Vector3.zero,Quaternion.identity);
                instantiatedPrefab.transform.SetParent(elementWrapper,false);

                uiElements.Add(instantiatedPrefab);
            }

            TextMeshProUGUI[] texts = uiElements[i].GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = el.playerName;
            texts[1].text = el.score.ToString();
        }
    }
    private void OnEnable()
    {
        HighScoreManager.onHighscoreListChanged += UpdateUI;
    }
    private void OnDisable()
    {
        HighScoreManager.onHighscoreListChanged -= UpdateUI;
    }
}
