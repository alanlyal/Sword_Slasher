using UnityEngine;

public class Customization : MonoBehaviour
{
    [Header("Customizable Sprites")]
    [SerializeField] private GameObject option1;
    [SerializeField] private GameObject option2;

    private void Start()
    {
        EnableNone();
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
