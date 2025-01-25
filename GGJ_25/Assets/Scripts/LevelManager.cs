using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [HideInInspector]
    public List<Bubble> bubblesInLevel = new List<Bubble>();
    public Rigidbody2D[] movedObjects;
    private Vector3[] startPositionsOfMovedObjects;

    public BubbleController mainBubble;
    // Start is called before the first frame update
    void Start()
    {
        startPositionsOfMovedObjects = new Vector3[movedObjects.Length];
        for (int i = 0; i < movedObjects.Length; i++)
        {
            startPositionsOfMovedObjects[i] = movedObjects[i].transform.position;
        }
    }

    public void ResetLevel()
    {
        for (int i = bubblesInLevel.Count - 1; i >= 0; i--)
        {
            bubblesInLevel[i].transform.position = bubblesInLevel[i].startPosition;
            bubblesInLevel[i].gameObject.SetActive(true);
            if (bubblesInLevel[i].isSpawned)
            {
                Destroy(bubblesInLevel[i].gameObject);
            }
        }

        for (int i = 0; i < movedObjects.Length; i++)
        {
            movedObjects[i].velocity = Vector3.zero;
            movedObjects[i].position = startPositionsOfMovedObjects[i];
        }


    }
}