using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Quest pool")]
    [SerializeField] private List<QuestSO> pool = new();
    [SerializeField] private int dailyCount = 3;
    [SerializeField] private int weeklyCount = 0;

    [Header("Optional")]
    [SerializeField] private UnlockToast toast;
    [SerializeField] private float rotationCheckInterval = 30f;

    public event Action OnQuestsChanged;
    public event Action<QuestSO> OnQuestCompleted;

    private readonly List<QuestState> active = new();
    private readonly Dictionary<string, QuestSO> lookup = new();
    private readonly Dictionary<EventType, int> lastKnownTotals = new();

    private DateTime dailyReset;
    private DateTime weeklyReset;
    private bool initialized;
    private float checkTimer;

    public IReadOnlyList<QuestState> Active => active;
    public DateTime DailyReset => dailyReset;
    public DateTime WeeklyReset => weeklyReset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        foreach (var q in pool)
            lookup[q.id] = q;
    }

    private void Start()
    {
        if (!initialized) Initialize(null, null);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void OnEnable()
    {
        if (Instance != this) return;
        GameEvents.OnEvent += HandleEvent;
    }

    private void OnDisable()
    {
        GameEvents.OnEvent -= HandleEvent;
    }

    private void Update()
    {
        if (!initialized) return;
        checkTimer += Time.unscaledDeltaTime;
        if (checkTimer < rotationCheckInterval) return;
        checkTimer = 0f;
        RefreshRotation();
    }

    public void Initialize(QuestSaveData saved, IReadOnlyDictionary<EventType, int> totals)
    {
        lastKnownTotals.Clear();
        if (totals != null)
            foreach (var kv in totals)
                lastKnownTotals[kv.Key] = kv.Value;

        active.Clear();
        dailyReset = DateTime.MinValue;
        weeklyReset = DateTime.MinValue;

        if (saved != null)
        {
            foreach (var s in saved.active)
            {
                if (!lookup.TryGetValue(s.questId, out var quest)) continue;
                if (s.objectives == null || s.objectives.Count != quest.objectives.Count) continue;
                active.Add(s);
            }
            dailyReset = new DateTime(saved.dailyResetUtcTicks, DateTimeKind.Utc);
            weeklyReset = new DateTime(saved.weeklyResetUtcTicks, DateTimeKind.Utc);
        }

        initialized = true;
        RefreshRotation();
        OnQuestsChanged?.Invoke();
    }

    public QuestSaveData GetSaveData() => new QuestSaveData
    {
        active = new List<QuestState>(active),
        dailyResetUtcTicks = dailyReset.Ticks,
        weeklyResetUtcTicks = weeklyReset.Ticks
    };

    public QuestSO GetQuest(QuestState state) => lookup[state.questId];

    public int GetProgress(QuestState state, int objectiveIndex)
    {
        var quest = lookup[state.questId];
        var obj = quest.objectives[objectiveIndex];
        var os = state.objectives[objectiveIndex];

        if (os.completed) return obj.targetValue;
        if (IsObjectiveLocked(state, objectiveIndex)) return 0;

        lastKnownTotals.TryGetValue(obj.type, out var current);
        return Mathf.Clamp(current - os.startValue, 0, obj.targetValue);
    }

    public bool IsObjectiveLocked(QuestState state, int objectiveIndex)
    {
        if (!lookup[state.questId].sequential) return false;
        for (int i = 0; i < objectiveIndex; i++)
            if (!state.objectives[i].completed) return true;
        return false;
    }

    public float GetOverallProgress01(QuestState state)
    {
        var quest = lookup[state.questId];
        if (quest.objectives.Count == 0) return state.completed ? 1f : 0f;

        float sum = 0f;
        for (int i = 0; i < quest.objectives.Count; i++)
            sum += (float)GetProgress(state, i) / quest.objectives[i].targetValue;
        return sum / quest.objectives.Count;
    }

    public bool Claim(QuestState state)
    {
        if (!state.completed || state.claimed) return false;

        state.claimed = true;
        foreach (var reward in lookup[state.questId].rewards)
            if (reward != null) reward.Grant();

        OnQuestsChanged?.Invoke();
        return true;
    }

    private void HandleEvent(EventType type, int total)
    {
        lastKnownTotals[type] = total;
        if (!initialized) return;

        foreach (var state in active)
        {
            if (state.completed) continue;
            var quest = lookup[state.questId];

            for (int i = 0; i < quest.objectives.Count; i++)
            {
                var obj = quest.objectives[i];
                var os = state.objectives[i];
                if (os.completed) continue;

                if (obj.type == type && total - os.startValue >= obj.targetValue)
                {
                    os.completed = true;

                    if (quest.sequential && i + 1 < quest.objectives.Count)
                    {
                        lastKnownTotals.TryGetValue(quest.objectives[i + 1].type, out var baseline);
                        state.objectives[i + 1].startValue = baseline;
                    }
                }
                if (quest.sequential) break;
            }
            if (state.objectives.TrueForAll(o => o.completed))
            {
                state.completed = true;
                if (toast != null) toast.Show($"Quest Complete: {quest.questName}", quest.description);
                OnQuestCompleted?.Invoke(quest);
            }
        }
        OnQuestsChanged?.Invoke();
    }

    private void RefreshRotation()
    {
        var now = DateTime.UtcNow;
        bool changed = false;

        if (now >= dailyReset)
        {
            Reassign(QuestPeriod.Daily, dailyCount);
            dailyReset = NextDailyReset(now);
            changed = true;
        }
        if (now >= weeklyReset)
        {
            Reassign(QuestPeriod.Weekly, weeklyCount);
            weeklyReset = NextWeeklyReset(now);
            changed = true;
        }

        if (EnsurePermanentQuests()) changed = true;

        if (changed) OnQuestsChanged?.Invoke();
    }

    private bool EnsurePermanentQuests()
    {
        bool added = false;
        foreach (var quest in pool)
        {
            if (quest.period != QuestPeriod.Permanent || quest.objectives.Count == 0) continue;
            if (active.Exists(s => s.questId == quest.id)) continue;

            active.Add(CreateState(quest));
            added = true;
        }
        return added;
    }

    private void Reassign(QuestPeriod period, int count)
    {
        active.RemoveAll(s => lookup[s.questId].period == period);
        if (count <= 0) return;

        var candidates = pool.FindAll(q => q.period == period && q.objectives.Count > 0);
        for (int i = candidates.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
        }

        for (int i = 0; i < Mathf.Min(count, candidates.Count); i++)
            active.Add(CreateState(candidates[i]));
    }

    private QuestState CreateState(QuestSO quest)
    {
        var state = new QuestState { questId = quest.id };
        foreach (var obj in quest.objectives)
        {
            lastKnownTotals.TryGetValue(obj.type, out int baseline);
            state.objectives.Add(new ObjectiveState { startValue = baseline });
        }
        return state;
    }

    private static DateTime NextDailyReset(DateTime nowUtc)
    {
        return nowUtc.ToLocalTime().Date.AddDays(1).ToUniversalTime();
    }

    private static DateTime NextWeeklyReset(DateTime nowUtc)
    {
        var local = nowUtc.ToLocalTime();
        int daysUntilMonday = ((int)DayOfWeek.Monday - (int)local.DayOfWeek + 7) % 7;
        if (daysUntilMonday == 0) daysUntilMonday = 7;
        return local.Date.AddDays(daysUntilMonday).ToUniversalTime();
    }
}