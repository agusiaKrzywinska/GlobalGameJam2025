using UnityEngine;

[CreateAssetMenu(fileName = "Poem Directory")]
public class PoemManager : ScriptableObject
{
    [TextArea]
    public string[] poems;
}
