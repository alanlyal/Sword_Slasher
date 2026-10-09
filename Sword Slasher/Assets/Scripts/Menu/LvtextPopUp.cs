using UnityEngine;
using UnityEngine.EventSystems;
public class LvtextPopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject popUpText;
    private void Start()
    {
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
        popUpText.SetActive(true);
    }
    public void OnPointerExit(PointerEventData mouseHover)
    {
        popUpText.SetActive(false);
    }
}

