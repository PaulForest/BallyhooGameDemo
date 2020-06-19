using UnityEngine;

public class Player
{
    private const string LastLevelKey = "LastLevelPlayed";

    public static Player Instance
    {
        get => Instance ?? (Instance = new Player());
        private set => Instance = value;
    }

    public string LastLevelPlayed { get; set; }

    private Player()
    {
        if (PlayerPrefs.HasKey(LastLevelKey))
        {
            // Get all data from player prefs
            LastLevelPlayed = PlayerPrefs.GetString(LastLevelKey);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetString(LastLevelKey, LastLevelPlayed);
        PlayerPrefs.Save();
    }
}
