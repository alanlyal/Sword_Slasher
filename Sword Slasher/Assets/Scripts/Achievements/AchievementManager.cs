using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [SerializeField] private List<AchievementSO> achievements;
    [SerializeField] private AchievementToast toast;

    private HashSet<string> unlocked = new();

    public IReadOnlyList<AchievementSO> All => achievements;
    public bool isUnlocked(AchievementSO a) => unlocked.Contains(a.name);

    private void Awake() => Instance = this;

    private void OnEnable()
    {
        GameEvents.OnLevelUp += CheckLevelUp;
        GameEvents.OnClick += CheckClick;
        GameEvents.OnUpgradePurchased += CheckUpgradePurchased;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelUp -= CheckLevelUp;
        GameEvents.OnClick -= CheckClick;
        GameEvents.OnUpgradePurchased -= CheckUpgradePurchased;
    }

    void CheckLevelUp(int levels)
    {
        foreach (var a in achievements)
            if (a.type == AchievementType.TotalLevel && levels >= a.targetValue)
                Unlock(a);
    }

    void CheckClick(int clicks)
    {
        foreach (var a in achievements)
            if (a.type == AchievementType.TotalClicks && clicks >= a.targetValue)
                Unlock(a);
    }

    void CheckUpgradePurchased(int upgrades)
    {
        foreach (var a in achievements)
            if (a.type == AchievementType.UpgradesPurchased && upgrades >= a.targetValue)
                Unlock(a);
    }

    void Unlock(AchievementSO achievement)
    {
        if (unlocked.Contains(achievement.name)) return;
        unlocked.Add(achievement.name);
        if (toast != null) toast.Show(achievement);
        Debug.Log($"Achievement Unlocked: {achievement.achievementName}");
    }
}
