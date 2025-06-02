using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    [HideInInspector] public int score;
    [HideInInspector] public int ignitions;

    Movement movement;

    void Start()
    {
        movement = GameObject.FindGameObjectWithTag("Agent").GetComponent<Movement>();
    }

    public void IncrementScore(int addedScore)
    {
        if (!movement.allowMovement) return;

        score = addedScore + score < 0 ? 0 : addedScore + score;
        scoreText.text = "score: " + score.ToString();
    }

    public void IncrementIgnition()
    {
        if (!movement.allowMovement) return;
        ignitions++;
    }
}
