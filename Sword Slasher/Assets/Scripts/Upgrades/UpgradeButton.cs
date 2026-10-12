using UnityEngine;
using TMPro;

//UI Button that purchases Upgrade
public class UpgradeButton : MonoBehaviour
{
    public UpgradeData data; //Add Any Upgrade

    private int amountOwned = 0; // Track how many times upgrade purchased (if purchasable multiple times)
    private int currentPrice;

    //For Display
    public TMP_Text nameText;
    public TMP_Text priceText;
    public TMP_Text ownedText;

    private void Start()
    {
        amountOwned = 0;
        currentPrice = data.basePrice;
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
            GameEvents.UpgradePurchased(amountOwned);

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
}

