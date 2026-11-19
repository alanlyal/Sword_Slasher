using UnityEngine;

[CreateAssetMenu(fileName = "XpReward", menuName = "Sword Slasher/Rewards/XP")]
public class XpRewardSO : RewardSO
{
    public int amount;
    public override string Describe() => $"+{amount} XP";
    public override void Grant()
    {
        LevelMenu.Instance.AddXp(amount);
        Debug.Log($"Granted {amount} XP");
    }
}