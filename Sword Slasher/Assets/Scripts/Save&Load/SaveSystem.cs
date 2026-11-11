using System.IO;
using UnityEngine;
public static class SaveSystem 
{
    public static PlayerData playerData = new PlayerData();
    public static bool isContinuing;
    private static string saveFile => Path.Combine(Application.persistentDataPath, "playerdata.json");
    public static void saveData()
    {
        File.WriteAllText(saveFile, JsonUtility.ToJson(playerData, true));
    }
    public static void LoadPlayerData()
    {
        if (!File.Exists(saveFile))

        {
            Debug.Log("no saved file found");
            return;
        }
        playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(saveFile));
    }
    public static bool hasPlayerDataSaved()
    {
        return File.Exists(saveFile);
    }
    public static int GetLevel()
    {
        return playerData.level;
    }
}