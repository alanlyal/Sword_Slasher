using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestObjectiveRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private CanvasGroup group; // optional

    public void Setup(string label, int progress, int target, bool done, bool locked)
    {
        labelText.text = label;
        countText.text = $"{progress}/{target}";
        progressBar.value = target > 0 ? (float)progress / target : 0f;

        if (group != null) group.alpha = locked ? 0.4f : 1f;
    }
}
