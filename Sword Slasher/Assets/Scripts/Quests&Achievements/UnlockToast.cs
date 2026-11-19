using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockToast : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private float displayTime = 3f;
    [SerializeField] private float gapTime = 0.5f;

    private readonly Queue<(string title, string description)> queue = new();
    private bool isShowing;

    public void Show(string title, string description)
    {
        queue.Enqueue((title, description));

        if (!isShowing)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (queue.Count > 0)
        {
            var (title, description) = queue.Dequeue();
            nameText.text = title;
            descText.text = description;

            panel.SetActive(true);
            yield return new WaitForSecondsRealtime(displayTime);
            panel.SetActive(false);

            yield return new WaitForSecondsRealtime(gapTime);
        }

        isShowing = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isShowing = false;
        queue.Clear();
        panel.SetActive(false);
    }
}
