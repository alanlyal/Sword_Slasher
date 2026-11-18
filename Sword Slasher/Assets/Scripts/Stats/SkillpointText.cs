using TMPro;
using UnityEngine;

public class SkillpointText : MonoBehaviour
{
    [SerializeField] private TMP_Text skillPointsText;

    private void Update()
    {
        var lm = LevelMenu.Instance;
        if (lm != null)
            UpdateStats(lm);
    }

    private void UpdateStats(LevelMenu lm)
    {
        skillPointsText.text = $"Skill Points: {lm.skillPoints}";
    }
}
