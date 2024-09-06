using UnityEngine;

public static class DataKey
{
    #region int

    public const string HighestScore = "HighestScore";
    public const string Use_Vibrate = "Use_Vibrate";
    public const string Use_Music = "Use_Music";
    public const string Use_SFX = "Use_Sound";
    
    public const string Ticket = "Ticket";
    public const string RemoveBoost = "RemoveBoost";
    public const string BoomBoost = "BoomBoost";
    public const string UpgradeBoost = "UpgradeBoost";
    public const string ShakeBoost = "ShakeBoost";
    

    #endregion

    public static bool IsUseMusic()
    {
        return PlayerPrefs.GetInt(Use_Music) == 1; 
    }
    
    public static bool IsUseVibrate()
    {
        return PlayerPrefs.GetInt(Use_Vibrate) == 1; 
    }
    
    public static bool IsUseSound()
    {
        return PlayerPrefs.GetInt(Use_SFX) == 1; 
    }
}
