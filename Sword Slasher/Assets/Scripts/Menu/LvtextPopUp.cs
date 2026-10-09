using UnityEngine;
using UnityEngine.EventSystems;
public class LvtextPopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject popupText;
    private void Start()
    {
        popupText.SetActive(false);
    }
    private void OnDisable()
    {
        if(popupText != null)
        {
            popupText.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        popupText.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        popupText.SetActive(false);
    }
}

