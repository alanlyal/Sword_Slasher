using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [Header("leaderboard elements")]
    [SerializeField] private TMP_InputField username; 
    [SerializeField] private Transform leaderboard; // this is for the whole leaderboard itself like the empty gameobject with all the textfields as childs
    [SerializeField] private GameObject textFieldPrefab; // this is the textfield childs from leaderboards
    private List<leaderboardUser> users = new List<leaderboardUser>();
    private List<TMP_Text> userRows = new List<TMP_Text>();
    private leaderboardUser playerData;
    private void OnEnable()
    {
        GameEvents.OnEvent += HandleEvent;
    }
    private void OnDisable()
    {
        GameEvents.OnEvent -= HandleEvent;
    }

    private void HandleEvent(EventType type, int value)
    {
        if (type == EventType.TotalClicks)
            allowClicks(value);
    }

    private void allowClicks(int clickTrack)
    {
        if (playerData == null)
        {
            return;
        }
        playerData.clicks = clickTrack;
        ReloadLeaderboard();
    }

    private void Start()
    {
        AddPlayer("king Arthor", 999, false);
        AddPlayer("Sir Lancelot", 300, false);
        AddPlayer("Sir William Marshal", 200, false);
        AddPlayer("josh", 10, false);
        string playerName = "Player";
        if (SaveSystem.isContinuing && !string.IsNullOrEmpty(SaveSystem.playerData.username))
        {
            playerName = SaveSystem.playerData.username;
        }
        AddPlayer(playerName, 0, true);
        username.text = playerName;
        UpdateClicks();
        
    }
    public void AddPlayer(string name, int score, bool isPlayer)
    {
        leaderboardUser user = new leaderboardUser(name, score, isPlayer);
        users.Add(user);
        if (isPlayer)
        {
            playerData = user;
        }
        GameObject spawned = Instantiate(textFieldPrefab, leaderboard);
        userRows.Add(spawned.GetComponentInChildren<TMP_Text>());
    }
    public void UpdateClicks()
    {
        playerData.clicks = LevelMenu.Instance.totalClicks;
        ReloadLeaderboard();
    }
    public void ReloadLeaderboard()
    {
        users.Remove(playerData);
        int position = users.Count;
        for (int i = 0; i < users.Count; i++)
        {
            if (playerData.clicks >= users[i].clicks)
            {
                position = i;
                break;
            }
        }
        users.Insert(position, playerData);
        for (int i = 0; i < userRows.Count; i++)
        {
            userRows[i].text = (i + 1) + " | " + users[i].playerName + " - " + users[i].clicks + " clicks";
        }
    }
    public void UpdateUsername()
    {
        if (username.text.Length > 0)
        {
            playerData.playerName = username.text;
            SaveSystem.playerData.username = username.text;
            ReloadLeaderboard();
        }
    }

}
