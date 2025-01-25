using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [HideInInspector]
    public List<Bubble> bubblesInLevel = new List<Bubble>();
    public Rigidbody2D[] movedObjects;
    private Vector3[] startPositionsOfMovedObjects;
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
        foreach (var bubble in bubblesInLevel)
        {
            bubble.gameObject.SetActive(true);
        }
    }
}