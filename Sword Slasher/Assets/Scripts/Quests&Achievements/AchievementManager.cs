using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [SerializeField] private List<AchievementSO> achievements;
    [SerializeField] private UnlockToast toast;

    private HashSet<string> unlocked = new();
    private Dictionary<EventType, List<AchievementSO>> byType;

    public IReadOnlyList<AchievementSO> All => achievements;
    public bool isUnlocked(AchievementSO a) => unlocked.Contains(a.id);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        BuildIndex();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void BuildIndex()
    {
        byType = new Dictionary<EventType, List<AchievementSO>>();
        foreach (var a in achievements)
        {
            if (!byType.TryGetValue(a.type, out var list))
                byType[a.type] = list = new List<AchievementSO>();
            list.Add(a);
        }
    }

    private void OnEnable()
    {
        if (Instance != this) return;
        GameEvents.OnEvent += Check;
    }

    private void OnDisable() => GameEvents.OnEvent -= Check;

    private void Check(EventType type, int value)
    {
        if (!byType.TryGetValue(type, out var list)) return;
        foreach (var a in list)
            if (value >= a.targetValue)
                Unlock(a);
    }

    void Unlock(AchievementSO achievement)
    {
        if (!unlocked.Add(achievement.id)) return;
        if (toast != null) toast.Show(achievement.achievementName, achievement.description);
        Debug.Log($"Achievement Unlocked: {achievement.achievementName}");
    }
    public List<string> GetUnlocked() => new List<string>(unlocked);

    public void RestoreUnlocked(List<string> achievmentIds)
    {
        unlocked = new HashSet<string>(achievmentIds ?? new List<string>());
    }
}
