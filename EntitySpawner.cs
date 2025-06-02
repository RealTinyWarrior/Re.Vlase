using System.Collections;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [HideInInspector] public bool reverseVlazeWithAgent;
    [HideInInspector] public bool reverseAlly;
    public GameObject[] entities;
    public float reachMaximumDifficultyIn = 30f;
    public float minSpawnTime;
    public float maxSpawnTime;
    public float maxLowerBound = 0.1f;
    public float maxUpperBound = 0.5f;

    [Header("Alien Spawner (1 in x)")]
    public int alienRate = 20;

    float rateOfMin;
    float rateOfMax;

    // Controls the difficulty
    void Start()
    {
        StartCoroutine(SetEntitySpawner());
        rateOfMin = (minSpawnTime - maxLowerBound) / reachMaximumDifficultyIn;
        rateOfMax = (maxSpawnTime - rateOfMax) / reachMaximumDifficultyIn;
    }

    void Update()
    {
        if (minSpawnTime >= maxLowerBound) minSpawnTime -= rateOfMin * Time.deltaTime;
        if (maxSpawnTime >= maxUpperBound) maxSpawnTime -= rateOfMax * Time.deltaTime;
    }

    public IEnumerator SetEntitySpawner()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        while (true)
        {
            // Select a random spawnpoint around us at a distance of 25 world distance
            Vector2 position = Random.insideUnitCircle.normalized * 25;
            int mobChances = Random.Range(0, alienRate + 1);
            int id = mobChances > 0 ? 0 : 1;

            // Switch Probabilities
            if (reverseAlly) id = id == 0 ? 1 : 0;

            // Spawn either alien or vlase
            Spawn(position, id);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }

    public void Spawn(Vector2 position, int id)
    {
        Vector2 offset = Random.insideUnitCircle * Random.Range(3, 8.5f);
        GameObject entity = Instantiate(entities[id], position + offset, Quaternion.identity);

        // * Managing Reversifiers
        if (!reverseVlazeWithAgent && !reverseAlly) return;

        Vlaze vlaze = entity.GetComponent<Vlaze>();
        if (id == 0 && reverseVlazeWithAgent)
        {
            vlaze.agentSprite.SetActive(true);
            StartCoroutine(MakeVlazeTransparent(entity));
        }

        // Make Vlaze act like Alien and Vise Versa
        if (reverseAlly)
        {
            if (id == 0) vlaze.isAlien = true;
            else if (id == 1) vlaze.isAlien = false;
        }
    }

    IEnumerator MakeVlazeTransparent(GameObject entity)
    {
        // Fixes Race condition between Vlaze setting it's color

        yield return new WaitForSeconds(0.05f);
        if (entity != null) entity.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }
}
