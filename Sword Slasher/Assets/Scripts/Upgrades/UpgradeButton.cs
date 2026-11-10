using UnityEngine;
using TMPro;

//UI Button that purchases Upgrade
public class UpgradeButton : MonoBehaviour
{
    public UpgradeData data; //Add Any Upgrade
    private int amountOwned = 0; // Track how many times upgrade purchased (if purchasable multiple times)
    private int currentPrice;
    // for saving upgrades
    public string upgradeId => data.name;
    public int upgradesOwned => amountOwned;

    //For Display
    public TMP_Text nameText;
    public TMP_Text priceText;
    public TMP_Text ownedText;

    private void Start()
    {
        currentPrice = data.basePrice + data.priceIncrease * amountOwned;
        UpdateUI();
    }

    public void OnPurchase()
    {
        if (LevelMenu.Instance.skillPoints >= currentPrice)
        {
            //Purchase Upgrade with Skill Points
            LevelMenu.Instance.skillPoints -= currentPrice;
            LevelMenu.Instance.upgradesOwned++;
            amountOwned++;
            GameEvents.UpgradePurchased(LevelMenu.Instance.upgradesOwned);

            //Apply Upgrade
            LevelMenu.Instance.xpPerClick += data.clickIncrease;
            LevelMenu.Instance.passiveClick += data.passiveClickIncrease;

            //Increase Price
            currentPrice += data.priceIncrease;
            UpdateUI();
        }
    }

    //Update UI on Upgrade Screen
    private void UpdateUI()
    {
        nameText.text = data.upgradeName;
        priceText.text = "Price: " + currentPrice;
        ownedText.text = "x" + amountOwned;
    }

    //Restore Upgrades from Save Data
    public void RestoreUpgrades(int owned )
    {
        amountOwned = owned;
        currentPrice = data.basePrice + data.priceIncrease * owned;
        UpdateUI();
    }
}

