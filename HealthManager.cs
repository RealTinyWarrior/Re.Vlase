using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int health = 100;
    public TextMeshProUGUI healthUI;
    public Image deathPanel;
    public TextMeshProUGUI[] deathTexts;

    bool allowExitCheck = false;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (allowExitCheck)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                int oldScore = PlayerPrefs.GetInt("score", 0);

                // Saving high score
                if (gameManager.score > oldScore)
                {
                    PlayerPrefs.SetInt("score", gameManager.score);
                    PlayerPrefs.SetInt("ignition", gameManager.ignitions);
                }

                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void Damage(int damage)
    {
        if (health <= 0) return;

        int finalHealth = health - damage < 0 ? 0 : health - damage;
        StartCoroutine(UseHealthCoroutine(finalHealth, false, 0.002f));
    }

    public void Heal(int healing)
    {
        if (health <= 0) return;

        int finalHealth = health + healing > 100 ? 100 : health + healing;
        StartCoroutine(UseHealthCoroutine(finalHealth, true, 0.002f));
    }

    IEnumerator UseHealthCoroutine(int healthVal, bool increase, float rate)
    {
        bool condition = increase ? health < healthVal : health > healthVal;

        while (condition)
        {
            if (!increase)
            {
                health = health - 1 <= 0 ? 0 : health - 1;
                if (health <= 0) StartCoroutine(KillAgent());
            }

            else health = health + 1 >= 100 ? 100 : health + 1;

            healthUI.text = health.ToString() + "HP";
            condition = increase ? health < healthVal : health > healthVal;
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator KillAgent()
    {
        GetComponent<Movement>().allowMovement = false;
        yield return new WaitForSeconds(0.25f);

        deathPanel.DOFade(0.8f, 2.2f);

        deathTexts[1].text = "scored " + gameManager.score.ToString() + " with " + gameManager.ignitions.ToString() + " ignitions";

        foreach (TextMeshProUGUI text in deathTexts)
        {
            text.DOFade(1f, 2.2f);
        }

        yield return new WaitForSeconds(1.5f);
        allowExitCheck = true;
    }
}
