using ClassicGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenManager : MonoBehaviour
{
    public GameObject HighScoreText;
    public Text ScoreText;
    public Text NameFieldText;
    public GameObject SubmitButton;
    public GameObject HighScorePanel;

    void Start()
    {
        HighScoreManager.LoadAllLocalHighScores();
        ScoreText.text = "Score: " + GameManager.Score;

        //If new high Score
        if (isHighScore())
        {
            HighScoreText.SetActive(true);
            NameFieldText.transform.parent.gameObject.SetActive(true);
            SubmitButton.SetActive(true);
        }
        else
        {
            SetHighScorePanel();
            HighScorePanel.SetActive(true);
        }
    }

    public void SetHighScorePanel()
    {
        HighScoreManager.HighScoreHolder hsh;
        if (GameManager.GameMode == 0)
        {
            hsh = HighScoreManager.LocalHighscores;
        }
        else
        {
            hsh = HighScoreManager.LocalAdvancedHighscores;
        }

        Text names = HighScorePanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text scores = HighScorePanel.transform.GetChild(1).gameObject.GetComponent<Text>();

        names.text = "";
        scores.text = "";

        foreach(HighScoreManager.ScoreEntry e in hsh.Entries)
        {
            names.text += e.Name.Substring(0, Mathf.Min(e.Name.Length, 6) ) + "\n";
            scores.text += e.Score + "\n";
        }
    }

    public void SubmitHighScore()
    {
        if(GameManager.GameMode == 0)
        {
            HighScoreManager.LocalHighscores.InsertNew(GameManager.Score, NameFieldText.text);
        }
        else
        {
            HighScoreManager.LocalAdvancedHighscores.InsertNew(GameManager.Score, NameFieldText.text);
        }

        HighScoreManager.SaveAllLocalHighScores();
        NameFieldText.transform.parent.gameObject.SetActive(false);
        SubmitButton.SetActive(false);
        SetHighScorePanel();
        HighScorePanel.SetActive(true);
    }

    private bool isHighScore()
    {
        if(GameManager.GameMode == 0)
            return GameManager.Score > HighScoreManager.LocalHighscores.Entries[HighScoreManager.LocalHighscores.Entries.Count - 1].Score;
        return GameManager.Score > HighScoreManager.LocalAdvancedHighscores.Entries[HighScoreManager.LocalHighscores.Entries.Count - 1].Score;
    }

    public void SceneChange(string s)
    {
        string sc = s;
        if (s == "Game")
        {
            if(GameManager.GameMode == 0)
            {
                sc = "Classic" + s;
            }else
            {
                sc = "Advanced" + s;
            }
        }
        SceneConfiguration.SwitchSceneTo(sc);
    }

}
