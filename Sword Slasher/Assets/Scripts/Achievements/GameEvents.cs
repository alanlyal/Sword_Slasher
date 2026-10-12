using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<int> OnLevelUp;
    public static event Action<int> OnClick;
    public static event Action<int> OnUpgradePurchased;

    public static void LevelUp(int totalLevel) => OnLevelUp?.Invoke(totalLevel);
    public static void Click(int totalClicks) => OnClick?.Invoke(totalClicks);
    public static void UpgradePurchased(int totalUpgrades) => OnUpgradePurchased?.Invoke(totalUpgrades);
}
