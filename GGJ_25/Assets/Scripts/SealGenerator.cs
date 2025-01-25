using UnityEngine;

public class SealGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform[] generatedComponents;
    // Start is called before the first frame update
    void Start()
    {
        //generate seal
        for (int i = 0; i < generatedComponents.Length; i++)
        {
            int selectedElement = Random.Range(0, generatedComponents[i].childCount);
            for (int j = 0; j < generatedComponents[i].childCount; j++)
            {
                generatedComponents[i].GetChild(j).gameObject.SetActive(selectedElement == j);
            }
        }
    }
}
