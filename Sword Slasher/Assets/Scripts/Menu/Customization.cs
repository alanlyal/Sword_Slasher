using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Customization : MonoBehaviour
{
    [Header("Customizable Sprites")]
    [SerializeField] private GameObject option1;
    [SerializeField] private GameObject option2;
    
    [Header("Sword Options")]
    [SerializeField] private Animator swordAnimator;
    
    //for saving 
    private int selectedOption;
    public int OptionSelected => selectedOption;

    private void Start()
    {
        if (SaveSystem.isContinuing)
        {
            LoadSelectedOption();
        }
        else
        {
            EnableNone();
        }
    }

    public void EnableNone()
    {
        selectedOption = 0;
        option1.SetActive(false);
        option2.SetActive(false);
    }

    public void EnableOption1()
    {
        selectedOption = 1;
        option1.SetActive(true);

        option2.SetActive(false);
    }

    public void EnableOption2()
    {
        selectedOption = 2;
        option1.SetActive(false);

        option2.SetActive(true);
    }

    public void EnableWoodSword()
    {
        SaveSystem.playerData.swordType = 0;
        swordAnimator.SetBool("MetalSword", false);
        swordAnimator.Play("Idle");
    }

    public void EnableMetalSword()
    {
        SaveSystem.playerData.swordType = 1;
        swordAnimator.SetBool("MetalSword", true);
        swordAnimator.Play("MetalSwordIdle");
    }
    
    public void LoadSelectedOption()
    { 
        int savedOption = SaveSystem.playerData.custimization;
        if (savedOption== 1)
        {
            EnableOption1();
        }
        else if (savedOption == 2)
        {
            EnableOption2();
        }
        else
        {
            EnableNone();
        }
        int swordTypeSelected = SaveSystem.playerData.swordType;
        if (swordTypeSelected == 1)
        {
            EnableMetalSword();
        }
        else
        {
            EnableWoodSword();
        }
    }
}
