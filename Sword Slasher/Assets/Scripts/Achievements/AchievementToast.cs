using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementToast : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private float displayTime = 3f;
    [SerializeField] private float gapTime = 0.5f;

    private readonly Queue<AchievementSO> queue = new();
    private bool isShowing;

    public void Show(AchievementSO achievement)
    {
        queue.Enqueue(achievement);

        if (!isShowing)
            StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (queue.Count > 0)
        {
            var a = queue.Dequeue();
            nameText.text = a.achievementName;
            descText.text = a.description;

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
