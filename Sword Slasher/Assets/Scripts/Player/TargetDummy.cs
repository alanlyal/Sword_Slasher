using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    private SpriteRenderer sr;
    
    private bool canClick = true;

    [Header("Stats")]
    [SerializeField] private int clicksPerClick = 1; //in case we add dual wielding or something
    [SerializeField] private float clickCooldown = 0.15f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        //ensuring the values are what they should be at start
        clicksPerClick = 1;
        clickCooldown = 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (canClick)
        {
            LevelMenu.Instance.AddClick(clicksPerClick);
            StartCoroutine(ClickCooldown(clickCooldown));
        }
        else
        {
            Debug.Log("can't click ");
        }

    }

    private IEnumerator ClickCooldown(float i)
    {
        canClick = false;
        yield return new WaitForSeconds(i);
        canClick = true;
    }
}
