using UnityEngine;
using UnityEngine.InputSystem;

public class SwordCursor : MonoBehaviour
{
    //public Texture2D swordSprite;

    public float size = 2f;

    void Start()
    {
        //Cursor.SetCursor(swordSprite, new Vector2(0, 0), CursorMode.Auto);

        Cursor.visible = false;

        transform.localScale = new Vector3(size, size, 1);



    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();


        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        worldPos.z = 0;
        transform.position = worldPos;

        



    }
}
