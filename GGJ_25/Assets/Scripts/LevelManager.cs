using Febucci.UI.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [HideInInspector]
    public List<Bubble> bubblesInLevel = new List<Bubble>();
    public Rigidbody2D[] movedObjects;
    private Vector3[] startPositionsOfMovedObjects;
    private Quaternion[] startRotationsOfMovedObjects;
    [HideInInspector]
    public BubbleController mainBubble;
    [HideInInspector]
    public Cinemachine.CinemachineBrain brain;
    [HideInInspector]
    public CameraBrainEventsHandler brainHelper;

    [SerializeField]
    private PoemManager poems;
    [SerializeField]
    private TextMeshPro peomDisplayer;
    private TypewriterCore typeWriter;
    [NaughtyAttributes.Scene]
    public string nextLevel;
    // Start is called before the first frame update
    void Start()
    {
        brain = FindObjectOfType<Cinemachine.CinemachineBrain>();
        brainHelper = brain.GetComponent<CameraBrainEventsHandler>();

        peomDisplayer.text = poems.poems[Random.Range(0, poems.poems.Length)];

        typeWriter = peomDisplayer.GetComponent<TypewriterCore>();
        typeWriter.onTextShowed.AddListener(() => mainBubble.goNext = true);

        startPositionsOfMovedObjects = new Vector3[movedObjects.Length];
        for (int i = 0; i < movedObjects.Length; i++)
        {
            startPositionsOfMovedObjects[i] = movedObjects[i].transform.position;
        }

        startRotationsOfMovedObjects = new Quaternion[movedObjects.Length];
        for (int i = 0; i < movedObjects.Length; i++)
        {
            startRotationsOfMovedObjects[i] = movedObjects[i].transform.rotation;
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
            movedObjects[i].transform.rotation = startRotationsOfMovedObjects[i];
            movedObjects[i].position = startPositionsOfMovedObjects[i];
        }
    }
}