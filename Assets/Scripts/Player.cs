using UnityEngine;

public class Player
{
    private const string LastLevelKey = "LastLevelPlayed";

    public static Player Instance => _instance ?? (_instance = new Player());
    private static Player _instance;

    public int LastLevelPlayed { get; set; }

    private Player()
    {
        LastLevelPlayed = PlayerPrefs.HasKey(LastLevelKey)
            ? PlayerPrefs.GetInt(LastLevelKey)
            : LevelList.Instance.GetFirstLevelBuildIndex();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(LastLevelKey, LastLevelPlayed);
        PlayerPrefs.Save();
    }
}
