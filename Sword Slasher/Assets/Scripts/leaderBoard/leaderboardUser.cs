using UnityEngine;

public class leaderboardUser
{
    public string playerName;
    public bool player;
    public int clicks;
    public leaderboardUser(string name, int clicks, bool player = false)
    {
        this.playerName = name;
        this.player = player;
        this.clicks = clicks;
    }
}
