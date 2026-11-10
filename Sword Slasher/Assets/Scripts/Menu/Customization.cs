using UnityEngine;

public class Customization : MonoBehaviour
{
    [Header("Customizable Sprites")]
    [SerializeField] private GameObject option1;
    [SerializeField] private GameObject option2;

    //for saving 
    private int selectedOption;
    public int OptionSelected => selectedOption;

    private void Start()
    {
        if (SaveSystem.isContinuing)
        {
            int savedOption = SaveSystem.playerData.custimization;
            if (savedOption == 1)
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
        }
    }

    public void EnableNone()
    {
        option1.SetActive(false);
        option2.SetActive(false);
    }

    public void EnableOption1()
    {
        option1.SetActive(true);

        option2.SetActive(false);
    }

    public void EnableOption2()
    {
        option1.SetActive(false);

        option2.SetActive(true);
    }

}
