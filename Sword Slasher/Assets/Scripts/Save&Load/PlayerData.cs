using System;
using System.Collections.Generic;
[Serializable]
public class PlayerData
{
    public int level = 1;
    public int xp;
    public int levelXp = 100;
    public int clickingXp = 10;
    public int skillPoints;
    public int passiveClicks;
    public int totalClicks;
    public int custimization;
    public int upgradesBought;
    public List<string> achievements = new();
    public List<string> upgrades = new();
}