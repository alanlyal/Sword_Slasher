using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text clicksText;
    [SerializeField] private TMP_Text skillPointsText;
    [SerializeField] private TMP_Text xpPerClickText;
    [SerializeField] private TMP_Text passiveClicksText;
    [SerializeField] private TMP_Text ownedUpgradesText;

    private void OnEnable()
    {
        var lm = LevelMenu.Instance;
        if (lm != null)
            UpdateStats(lm);
    }

    private void UpdateStats(LevelMenu lm)
    {
        levelText.text = $"Level: {lm.level}";
        xpText.text = $"XP: {lm.xp} / {lm.levelXp}";
        clicksText.text = $"Total Clicks: {lm.totalClicks}";
        skillPointsText.text = $"Skill Points: {lm.skillPoints}";
        xpPerClickText.text = $"XP per Click: {lm.xpPerClick}";
        passiveClicksText.text = $"Passive Clicks/sec: {lm.passiveClick}";
        ownedUpgradesText.text = $"Upgrades Purchased: {lm.upgradesOwned}";
    }
}
