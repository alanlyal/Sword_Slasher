using System;
using System.Collections.Generic;

[Serializable]
public class ObjectiveState
{
    public int startValue;
    public bool completed;
}

[Serializable]
public class QuestState
{
    public string questId;
    public List<ObjectiveState> objectives = new();
    public bool completed;
    public bool claimed;
}

[Serializable]
public class QuestSaveData
{
    public List<QuestState> active = new();
    public long dailyResetUtcTicks;
    public long weeklyResetUtcTicks;
}
