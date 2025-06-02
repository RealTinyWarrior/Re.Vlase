using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReversifierManager : MonoBehaviour
{
    public float initiationTime = 5f;
    public float removifierMin = 5f;
    public float removifierMax = 16f;
    public float reversifierThreshold = 5f;

    public Light2D globalLight;
    public TextMeshProUGUI reversifierText;
    public SpriteRenderer[] reversableSprites;
    ReversifierPopup popup;
    EntitySpawner entitySpawner;
    SpriteRenderer agentSprite;
    Movement movement;
    Shooter shooter;

    readonly List<string> reversifierList = new() {
        "Neutral",
    };

    void Start()
    {
        entitySpawner = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EntitySpawner>();
        GameObject agent = GameObject.FindGameObjectWithTag("Agent");
        agentSprite = agent.GetComponent<SpriteRenderer>();
        movement = agent.GetComponent<Movement>();
        popup = GetComponent<ReversifierPopup>();
        shooter = agent.GetComponent<Shooter>();

        if (PlayerPrefs.GetInt("reversifier", 0) == 0) StartCoroutine(InitiateReversifiers());
        else reversifierText.color = new Color(0, 0, 0, 0);
    }

    IEnumerator InitiateReversifiers()
    {
        // Wait a few seconds before initializing
        yield return new WaitForSeconds(Random.Range(5f, 11f));

        while (movement.allowMovement)
        {
            int probability = Random.Range(-5, 9);

            // Neutral, Return!
            if (probability <= -3)
            {
                float duration = Random.Range(4f, 8f);
                yield return new WaitForSeconds(duration + reversifierThreshold);
            }

            // Single Reversifier
            else if (probability <= 2)
            {
                float duration = Random.Range(7f, 14f);
                CallSingleReversifier(Random.Range(0, 10), duration);

                yield return new WaitForSeconds(duration + reversifierThreshold);
            }

            // Double Reversifier
            else if (probability <= 7)
            {
                float duration = Random.Range(5f, 9f);
                CallSingleReversifier(Random.Range(0, 10), duration);

                int doubleProbabilityID = Random.Range(0, 9);

                CallSingleReversifier(doubleProbabilityID, duration);

                // Chroma Color, Randomize between 2 to 9 as Chroma Color doesn't support 10
                if (doubleProbabilityID == 1) CallSingleReversifier(Random.Range(2, 9), duration + reversifierThreshold);

                // Reversed Enemies, Randomize between 4 to 8 as this doesn't support the last 2
                else if (doubleProbabilityID == 3) CallSingleReversifier(Random.Range(4, 8), duration + reversifierThreshold);

                // Flashbang, which will just support Reversed Ally
                else if (doubleProbabilityID == 7) CallSingleReversifier(8, duration + reversifierThreshold);

                // Call the others
                else CallSingleReversifier(Random.Range(doubleProbabilityID + 1, 10), duration + reversifierThreshold);

                yield return new WaitForSeconds(duration + reversifierThreshold);
            }

            // Tripple Reversifiers
            else
            {
                int trippleProbability = Random.Range(0, 3);

                // Reversed Aim + Ammo + Machinegun
                if (trippleProbability == 0)
                {
                    float duration = Random.Range(3.5f, 6f);

                    CallSingleReversifier(0, duration);
                    CallSingleReversifier(2, duration);
                    CallSingleReversifier(6, duration);

                    yield return new WaitForSeconds(duration);
                }

                // ReversedParallax + Flashbang + chroma
                else if (trippleProbability == 1)
                {
                    float duration = Random.Range(3f, 8f);

                    CallSingleReversifier(5, duration + reversifierThreshold);
                    CallSingleReversifier(7, duration + reversifierThreshold);
                    CallSingleReversifier(1, duration + reversifierThreshold);

                    yield return new WaitForSeconds(duration);
                }

                // AIM + Enemies + FlashBang
                else
                {
                    float duration = Random.Range(5f, 8f);

                    CallSingleReversifier(0, duration);
                    CallSingleReversifier(3, duration);
                    CallSingleReversifier(7, duration);

                    yield return new WaitForSeconds(duration);
                }
            }
        }
    }

    // 0 Means a random duration determined by the function
    void CallSingleReversifier(int id, float duration)
    {
        switch (id)
        {
            case 0: StartCoroutine(ReverseAim(duration)); break;
            case 1: StartCoroutine(ChromaColor(duration)); break;
            case 2: StartCoroutine(ReverseAmmo(duration)); break;
            case 3: StartCoroutine(ReverseEnemy(duration)); break;
            case 4: StartCoroutine(ReverseControl(duration)); break;
            case 5: StartCoroutine(ReverseParallax(duration)); break;
            case 6: StartCoroutine(Machinegun(duration)); break;
            case 7: StartCoroutine(Flashbang(duration)); break;
            case 8: StartCoroutine(ReverseAlly(duration)); break;
            case 9: StartCoroutine(Blackout(duration)); break;
            default: Debug.LogWarning($"Invalid reversifier ID: {id}"); break;
        }
    }

    // ! NOTE that reverse ally and reverse enemy cannot happen simuntaniously

    IEnumerator ReverseParallax(float duration)
    {
        ChangeList("Reverse Parallax", true);
        foreach (SpriteRenderer sprite in reversableSprites)
        {
            sprite.flipY = true;
        }

        yield return new WaitForSeconds(duration);

        ChangeList("Reverse Parallax", false);
        foreach (SpriteRenderer sprite in reversableSprites)
        {
            sprite.flipY = false;
        }
    }

    IEnumerator Flashbang(float duration)
    {
        ChangeList("Flashbang", true);
        globalLight.intensity = 8;
        yield return new WaitForSeconds(duration);

        globalLight.intensity = 1;
        ChangeList("Flashbang", false);
    }

    IEnumerator Machinegun(float duration)
    {
        float neutralAttackDelay = shooter.attackDelay;

        shooter.attackDelay = 0.03f;
        shooter.isMachineGun = true;
        ChangeList("Machinegun", true);

        yield return new WaitForSeconds(duration);

        shooter.attackDelay = neutralAttackDelay;
        shooter.isMachineGun = false;
        ChangeList("Machinegun", false);
    }

    IEnumerator ReverseAlly(float duration)
    {
        entitySpawner.reverseAlly = true;
        ChangeList("Reverse Ally", true);

        yield return new WaitForSeconds(duration);

        entitySpawner.reverseAlly = false;
        ChangeList("Reverse Ally", false);
    }

    IEnumerator ReverseEnemy(float duration)
    {
        agentSprite.color = new Color(agentSprite.color.r, agentSprite.color.g, agentSprite.color.b, 0);
        entitySpawner.reverseVlazeWithAgent = true;
        movement.vlazeSprite.SetActive(true);
        ChangeList("Reverse Enemy", true);

        yield return new WaitForSeconds(duration);

        agentSprite.color = new Color(agentSprite.color.r, agentSprite.color.g, agentSprite.color.b, 1);
        entitySpawner.reverseVlazeWithAgent = false;
        movement.vlazeSprite.SetActive(false);
        ChangeList("Reverse Enemy", false);
    }

    IEnumerator Blackout(float duration)
    {
        ChangeList("Blackout", true);
        globalLight.color = new Color(0.01f, 0.01f, 0.01f, 1);
        yield return new WaitForSeconds(duration);

        globalLight.color = new Color(1, 1, 1, 1);
        ChangeList("Blackout", false);
    }

    IEnumerator ChromaColor(float duration)
    {
        float timePassed = 0;
        ChangeList("Chroma Color", true);

        while (timePassed < duration)
        {
            float hue = timePassed * 0.7f % 1f;
            globalLight.color = Color.HSVToRGB(hue, 1f, 1f);

            timePassed += Time.deltaTime;
            yield return null;
        }

        globalLight.color = new Color(1, 1, 1, 1);
        ChangeList("Chroma Color", false);
    }

    IEnumerator ReverseAmmo(float duration)
    {
        agentSprite.color = new Color(agentSprite.color.r, agentSprite.color.g, agentSprite.color.b, 0);
        movement.bulletSprite.SetActive(true);
        shooter.reverseAmmo = true;
        ChangeList("Reverse Ammo", true);

        yield return new WaitForSeconds(duration);

        agentSprite.color = new Color(agentSprite.color.r, agentSprite.color.g, agentSprite.color.b, 1);
        movement.bulletSprite.SetActive(false);
        shooter.reverseAmmo = false;
        ChangeList("Reverse Ammo", false);
    }

    IEnumerator ReverseAim(float duration)
    {
        shooter.reverseAim = true;
        ChangeList("Reverse Aim", true);

        yield return new WaitForSeconds(duration);

        shooter.reverseAim = false;
        ChangeList("Reverse Aim", false);
    }

    IEnumerator ReverseControl(float duration)
    {
        movement.reverseControls = true;
        ChangeList("Reverse Control", true);

        yield return new WaitForSeconds(duration);

        movement.reverseControls = false;
        ChangeList("Reverse Control", false);
    }

    public void ChangeList(string rName, bool add)
    {
        StartCoroutine(ChangeListCoroutine(rName, add));
    }

    IEnumerator ChangeListCoroutine(string rName, bool add)
    {
        // To Prevent any Race Case
        yield return new WaitForSeconds(0.05f);

        if (add && !reversifierList.Contains(rName))
        {
            reversifierList.Add(rName);
        }

        else if (!add) reversifierList.Remove(rName);

        // Set empty as Neutral
        if (reversifierList.Count > 0) reversifierList.Remove("Neutral");
        else reversifierList.Add("Neutral");

        // Add the Reversifiers in the string
        reversifierText.text = "";
        foreach (string reversifier in reversifierList) reversifierText.text += reversifier + ", ";

        if (reversifierText.text.Contains(", ")) reversifierText.text = reversifierText.text[0..(reversifierText.text.Length - 2)];
        popup.ShowPopup(reversifierText.text);
    }
}
