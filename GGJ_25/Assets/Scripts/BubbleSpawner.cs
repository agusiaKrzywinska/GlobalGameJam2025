using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    private Bubble[] possibleBubblesToSpawn;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float spawnRate = 1f;
    private float currentSpawnRate;

    private List<Bubble> spawnedBubbles = new List<Bubble>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.mainBubble.isInLauncher || LevelManager.Instance.mainBubble.isInEndZone)
        {
            currentSpawnRate = 0f;
            return;
        }

        currentSpawnRate += Time.deltaTime;
        if (currentSpawnRate >= spawnRate)
        {
            currentSpawnRate -= spawnRate;
            spawnedBubbles.Add(Instantiate(possibleBubblesToSpawn[Random.Range(0, possibleBubblesToSpawn.Length)], spawnPoint.position, Quaternion.identity));
        }
    }
}
