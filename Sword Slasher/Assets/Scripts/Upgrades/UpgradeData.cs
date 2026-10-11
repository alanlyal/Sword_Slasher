using UnityEngine;

//Create Any Upgrade
[CreateAssetMenu(fileName = "New Upgrade", menuName = "Sword Slasher/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public int basePrice = 1;
    public int priceIncrease = 1;

    public int clickIncrease = 0; //Increase damage per click
    public int passiveClickIncrease = 0; //Passive Income
    
}
