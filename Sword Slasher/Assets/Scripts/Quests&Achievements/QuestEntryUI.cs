using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Transform objectiveContainer;
    [SerializeField] private QuestObjectiveRowUI objectiveRowPrefab;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text timerText; // optional
    [SerializeField] private Button claimButton;
    [SerializeField] private TMP_Text claimButtonText;
    [SerializeField] private CanvasGroup group; // optional 

    private readonly List<QuestObjectiveRowUI> rows = new();
    private QuestSO quest;

    public QuestState State { get; private set; }

    public void Bind(QuestState state)
    {
        State = state;
        quest = QuestManager.Instance.GetQuest(state);

        titleText.text = quest.questName;
        rewardText.text = string.Join(", ", quest.rewards.Where(r => r != null).Select(r => r.Describe()));

        foreach (var _ in quest.objectives)
            rows.Add(Instantiate(objectiveRowPrefab, objectiveContainer));

        claimButton.onClick.AddListener(() => QuestManager.Instance.Claim(State));
    }

    public void Refresh()
    {
        var qm = QuestManager.Instance;

        for (int i = 0; i < rows.Count; i++)
        {
            var obj = quest.objectives[i];
            rows[i].Setup(obj.DisplayLabel, qm.GetProgress(State, i), obj.targetValue, State.objectives[i].completed, qm.IsObjectiveLocked(State, i));
        }

        claimButton.interactable = State.completed && !State.claimed;
        if (group != null) group.alpha = State.claimed ? 0.5f : 1f;

        if (State.claimed) claimButtonText.text = "Claimed";
        else
        {
            claimButtonText.text = "Claim";
        }

        RefreshTimer();
    }

    public void RefreshTimer()
    {
        if (timerText == null) return;
        var qm = QuestManager.Instance;

        DateTime reset;
        switch (quest.period)
        {
            case QuestPeriod.Daily: reset = qm.DailyReset; break;
            case QuestPeriod.Weekly: reset = qm.WeeklyReset; break;
            default:
                timerText.gameObject.SetActive(false);
                return;
        }

        timerText.gameObject.SetActive(true);
        timerText.text = FormatTime(reset - DateTime.UtcNow);
    }

    private static string FormatTime(TimeSpan t)
    {
        if (t < TimeSpan.Zero) t = TimeSpan.Zero;
        if (t.TotalDays >= 1) return $"{t.Days}D {t.Hours}H";
        return $"{(int)t.TotalHours}H {t.Minutes}M";
    }
}
