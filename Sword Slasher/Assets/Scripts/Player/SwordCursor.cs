using UnityEngine;
using UnityEngine.InputSystem;

public class SwordCursor : MonoBehaviour
{
    //public Texture2D swordSprite;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject customizationPanel;
    public float size = 2f;

    void Start()
    {
        //Cursor.SetCursor(swordSprite, new Vector2(0, 0), CursorMode.Auto);
        transform.localScale = new Vector3(size, size, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if a panel is open to display the cursor
        if (tutorialPanel.activeSelf || optionsPanel.activeSelf || statsPanel.activeSelf || upgradePanel.activeSelf || customizationPanel.activeSelf)
        {
            Cursor.visible = true;
            return;
        }

        Cursor.visible = false;

        Vector2 mousePos = Mouse.current.position.ReadValue();


        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        worldPos.z = 0;
        transform.position = worldPos;
    }
}
