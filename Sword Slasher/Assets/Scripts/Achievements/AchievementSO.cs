using UnityEngine;

[CreateAssetMenu(fileName = "AchievementSO", menuName = "Sword Slasher/Achievement")]
public class AchievementSO : ScriptableObject
{
    public string achievementName;
    [TextArea] public string description;
    public AchievementType type;
    public int targetValue;
}

public enum AchievementType
{
    TotalLevel,
    TotalClicks,
    UpgradesPurchased
}
