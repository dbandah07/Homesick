using UnityEngine;

[CreateAssetMenu(fileName = "NewMemory", menuName = "Homesick/Memory")]
public class MemoryData : ScriptableObject
{
    public string openingLine = "This reminds me of...";

    [TextArea(2, 4)]
    public string[] lines;
}