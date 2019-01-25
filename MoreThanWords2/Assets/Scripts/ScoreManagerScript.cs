using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerScript : MonoBehaviour {
    public string NextScene;
    public string TextoSiguienteEscena;
    public int totalScore = 0;
    [SerializeField] private int killScore = 0;
    [SerializeField] private int negativeScore = 0;
    [SerializeField] private int timeScore = 0;
    private float timer = 0;

    public void ResetAll()
    {
        ResetScore();
        ResetTimer();
    }

    public void AddKillPoints(int points)
    {
        killScore += points;
    }

    public void AddNegativePoints(int points)
    {
        negativeScore += points;
    }

    public void CalculateTotalScore()
    {
        CalculateTimeScore();
        totalScore = timeScore + killScore - negativeScore;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    private void ResetTimer()
    {
        timer = 0;
    }

    internal string getNextScene()
    {
        return NextScene;
    }

    private void ResetScore()
    {
        totalScore = 0;
        killScore = 0;
        negativeScore = 0;
    }

    private void CalculateTimeScore()
        //POR DETERMINAR LO QUE QUEREMOS DAR AUN - BASE SIMPLE
    {
        if (timer > 120)
        {
            timeScore = 0;
        }
        else
        {
            timeScore = (int)(120 - timer)*2;
        }
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }

    public int getKillScore()
    {
        return killScore;
    }

    public int getTotalScore()
    {
        CalculateTotalScore();
        return totalScore;
    }

    public int getTimeScore()
    {
        CalculateTimeScore();
        return timeScore;
    }

    public int getNegativeScore()
    {
        return -negativeScore;
    }

    public string getStoryText()
    {
        return TextoSiguienteEscena;
    }
}
