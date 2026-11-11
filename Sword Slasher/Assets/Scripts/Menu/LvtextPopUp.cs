using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class LvtextPopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject popUpText;
    private TMP_Text continueLv;
    private void Start()
    {
        continueLv= popUpText.GetComponent<TMP_Text>();
        popUpText.SetActive(false);
    }
    private void OnDisable()
    {
        if(popUpText != null)
        {
            popUpText.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData mouseHover)
    {
        continueLv.text = "Level: " + SaveSystem.GetLevel().ToString();
        popUpText.SetActive(true);
    }
    public void OnPointerExit(PointerEventData mouseHover)
    {
        popUpText.SetActive(false);
    }
}

