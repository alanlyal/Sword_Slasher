using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardManager : MonoBehaviour
{
    [Header("leaderboard elements")]
    [SerializeField] private InputField username; // player will give themself a name TODO save this 
    [SerializeField] private Transform leaderboard; // this is for the whole leaderboard itself like the empty gameobject with all the textfields as childs
    [SerializeField] private GameObject textFieldPrefab; // this is the textfield childs from leaderboards
    private List<leaderboardUser> users = new List<leaderboardUser>();
    private List<Text> userRows = new List<Text>();
    private leaderboardUser playerData;
    private void Start()
    {
        AddPlayer("king Arthor", 999, false);
        AddPlayer("Sir Lancelot", 300, false);
        AddPlayer("Sir William Marshal", 200, false);
        AddPlayer("josh", 10, false);
        AddPlayer("Player", 0, true);
        username.text = "Player";
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
        userRows.Add(spawned.GetComponentInChildren<Text>());
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
            ReloadLeaderboard();
        }
    }
}
