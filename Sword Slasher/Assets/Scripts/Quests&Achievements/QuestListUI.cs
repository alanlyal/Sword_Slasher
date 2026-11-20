using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private QuestEntryUI entryPrefab;
    [SerializeField] private GameObject emptyLabel; // optional, "No quests right now" prefab

    private readonly List<QuestEntryUI> entries = new();
    private bool subscribed;
    private float timerClock;

    private void OnEnable() => TrySubscribe();

    private void Start() => TrySubscribe();

    private void OnDisable()
    {
        if (subscribed && QuestManager.Instance != null)
            QuestManager.Instance.OnQuestsChanged -= Refresh;
        subscribed = false;
    }

    private void TrySubscribe()
    {
        if (subscribed) return;
        var qm = QuestManager.Instance;
        if (qm == null) return;

        qm.OnQuestsChanged += Refresh;
        subscribed = true;
        Refresh();
    }

    private void Update()
    {
        timerClock += Time.unscaledDeltaTime;
        if (timerClock < 1f) return;
        timerClock = 0f;
        foreach (var e in entries) e.RefreshTimer();
    }

    private void Refresh()
    {
        var ordered = QuestManager.Instance.Active.OrderBy(s => s.claimed).ToList();

        bool same = entries.Count == ordered.Count;
        for (int i = 0; same && i < entries.Count; i++)
            same = entries[i].State == ordered[i];

        if (!same)
        {
            foreach (var e in entries) Destroy(e.gameObject);
            entries.Clear();

            foreach (var state in ordered)
            {
                var entry = Instantiate(entryPrefab, content);
                entry.Bind(state);
                entries.Add(entry);
            }
        }

        foreach (var e in entries) e.Refresh();
        if (emptyLabel != null) emptyLabel.SetActive(entries.Count == 0);
    }
}
