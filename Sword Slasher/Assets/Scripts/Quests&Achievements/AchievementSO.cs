using UnityEngine;

[CreateAssetMenu(fileName = "AchievementSO", menuName = "Sword Slasher/Achievement")]
public class AchievementSO : ScriptableObject
{
    public string id;
    public string achievementName;
    [TextArea] public string description;
    public EventType type;
    public int targetValue;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id)) id = name;
    }
#endif
}