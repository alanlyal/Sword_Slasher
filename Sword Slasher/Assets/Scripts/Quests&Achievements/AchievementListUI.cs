using System.Collections.Generic;
using UnityEngine;

public class AchievementListUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private AchievementEntryUI entryPrefab;

    private readonly List<AchievementEntryUI> entries = new();

    private void OnEnable()
    {
        var mgr = AchievementManager.Instance;
        if (mgr == null) return;

        if (entries.Count == 0)
            foreach (var a in mgr.All)
                entries.Add(Instantiate(entryPrefab, content));

        for (int i = 0; i < entries.Count; i++)
        {
            entries[i].Setup(mgr.All[i], mgr.isUnlocked(mgr.All[i]));
        }
    }
}
