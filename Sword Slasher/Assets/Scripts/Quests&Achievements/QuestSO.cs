using System;
using System.Collections.Generic;
using UnityEngine;

public enum QuestPeriod
{
    Daily,
    Weekly,
    Permanent
}

[Serializable]
public class QuestObjective
{
    public EventType type;
    public int targetValue;
    [Tooltip("Optional UI text. Falls back to type + target if empty.")]
    public string label;

    public string DisplayLabel => string.IsNullOrEmpty(label) ? $"{type} {targetValue}" : label;
}

[CreateAssetMenu(fileName = "QuestSO", menuName = "Sword Slasher/Quest")]
public class QuestSO : ScriptableObject
{
    public string id;
    public string questName;
    [TextArea] public string description;
    public QuestPeriod period;

    public bool sequential;
    public List<QuestObjective> objectives = new();

    public List<RewardSO> rewards = new();


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id)) id = name;
    }
#endif
}