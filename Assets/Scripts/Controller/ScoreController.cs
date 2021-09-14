using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [Header("Score Highlight")]
    public int scoreHighlightRange;
    public PlayerSoundController sound;

    private int currentScore;
    private int lastScoreHighlight;

    void Start(){
        currentScore = 0;
        lastScoreHighlight = 0;
    }

    public float GetCurrentScore() {
        return currentScore;
    }

    public void IncreaseCurrentScore(int increment) {
        currentScore += increment;
        if (currentScore - lastScoreHighlight > scoreHighlightRange) {
            sound.PlayScoreHighlight();
            player.maxSpeed = Mathf.Clamp(player.speedMultiplier * player.maxSpeed, player.maxSpeed, player.limitMaxSpeed);
            lastScoreHighlight += scoreHighlightRange;
        }
    }

    public void FinishScoring() { 
        if (currentScore > ScoreData.highScore) { ScoreData.highScore = currentScore; }
    }
}
