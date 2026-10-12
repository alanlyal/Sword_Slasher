using TMPro;
using UnityEngine;

public class AchievementEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private CanvasGroup group;

    public void Setup(AchievementSO a, bool isUnlocked)
    {
        nameText.text = isUnlocked ? a.achievementName : "???";
        descText.text = a.description;
        group.alpha = isUnlocked ? 1f : 0.4f;
    }
}
